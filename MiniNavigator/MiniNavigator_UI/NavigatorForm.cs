using Infralution.Controls.VirtualTree;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Service;
using MiniNavigator_Services.Service.Interface;
using MiniNavigator_UI.Mapper;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.Service;
using MiniNavigator_UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI
{
    public partial class NavigatorForm : Form
    {
        private readonly IObjectService _objectService;
        private readonly IObjectTypeService _objectTypeService;
        private readonly IValidationService _validationService;

        private readonly IMapper<NavObjectViewModel, NavObjectDTO> _objectMapper;

        private NavObjectViewModel _treeOfObjects = new NavObjectViewModel();

        private DataTable _table;

        public NavigatorForm(
            IObjectService objectService,
            IObjectTypeService objectTypeService,
            IValidationService validationService,
            IMapper<NavObjectViewModel, NavObjectDTO> objectMapper)
        {
            _objectService = objectService;
            _objectTypeService = objectTypeService;
            _validationService = validationService;

            _objectMapper = objectMapper;

            InitializeComponent();

            NavigatorDataGridView.RowHeaderMouseClick += NavigatorDataGridView_RowHeaderMouseClick;
            NavigatorDataGridView.CellClick += NavigatorDataGridView_CellClick;
            NavigatorDataGridView.ColumnHeaderMouseClick += NavigatorDataGridView_ColumnHeaderMouseClick;
            NavigatorDataGridView.CellValidating += NavigatorDataGridView_CellValidating;

            this.Load += NavigatorForm_Load;
        }

        private void NavigatorDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!IsEditing) return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var value = e.FormattedValue?.ToString();

            if (string.IsNullOrWhiteSpace(value))
                return;

            string error;
            bool valid = _validationService.ValidateSingleValue(
                value,
                column.ValueType,
                out error
            );

            if (!valid)
            {
                e.Cancel = true;
                MessageBox.Show(error, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void NavigatorDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SetTableReadonlyProperty();
        }

        private async void NavigatorForm_Load(object sender, EventArgs e)
        {
            BindTree();

            await InitRootAsync();

            NavigatorVirtualTree.DataSource = _treeOfObjects;
            NavigatorVirtualTree.Refresh();
        }

        private void BindTree()
        {
            var binding = new ObjectRowBinding(typeof(NavObjectViewModel));

            binding.ChildProperty = nameof(NavObjectViewModel.Children);

            var nameBinding = new ObjectCellBinding
            {
                Column = NavigatorVirtualTree.Columns[0],
                Field = nameof(NavObjectViewModel.ObjectTitle)
            };

            binding.CellBindings.Add(nameBinding);

            NavigatorVirtualTree.RowBindings.Add(binding);
        }

        private async Task InitRootAsync()
        {
            var rootDto = await _objectService.GetTreeOfObjectsAsync();
            _treeOfObjects = ConvertTreeToViewModels(rootDto);
        }

        public NavObjectViewModel ConvertTreeToViewModels(NavObjectDTO root)
        {
            var result = _objectMapper.ToViewModel(root);

            if (root.Children != null && root.Children.Any())
            {
                foreach (var child in root.Children)
                {
                    result.Children.Add(ConvertTreeToViewModels(child));
                }
            }

            return result;
        }

        private async void NavigatorVirtualTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsEditing)
                return;
            if (NavigatorVirtualTree.SelectedRow?.Item is NavObjectViewModel navObjectViewModel)
            {
                if (e.Button == MouseButtons.Right)
                {
                    await ShowContextMenuAsync(navObjectViewModel, e.Location);
                }
                else if (e.Button == MouseButtons.Left)
                {
                    await BindTableAsync(navObjectViewModel.ID);
                }
            }
        }

        private async Task ShowContextMenuAsync(NavObjectViewModel navObjectViewModel, Point location)
        {
            var actions = await _objectService.GetActionsForObject(navObjectViewModel.ID);

            ContextMenuStrip menu = new ContextMenuStrip();
            foreach (var action in actions)
            {
                var item = new ToolStripMenuItem(action.CommandName) { Tag = action };
                menu.Items.Add(item);
                item.Click += ItemAdd_Click;
            }

            menu.Show(NavigatorVirtualTree, location);
        }

        private void ItemAdd_Click(object sender, EventArgs e)
        {
            if (NavigatorVirtualTree.SelectedRow?.Item is NavObjectViewModel navObjectViewModel && _table != null)
            {
                ResetSorting();
                SetTableReadonlyProperty();

                // Добавление новой строки и кнопок для ссылочных колонок
                AddNewRowWithButtons();

                IsEditing = true;                    // Переключаем режим редактирования
                RestrictGridDuringCreation();        // Ограничиваем сетку при создании
                InitializeCreateNewObject();         // Инициализация объекта для новой строки
                ShowControls();                      // Отображаем дополнительные элементы управления
            }
        }

        private void AddNewRowWithButtons()
        {
            if (_table == null) return;

            _newRow = _table.NewRow();
            foreach (DataColumn col in _table.Columns)
                _newRow[col.ColumnName] = DBNull.Value;

            _table.Rows.Add(_newRow);
            int rowIndex = _table.Rows.IndexOf(_newRow);

            var gridRow = NavigatorDataGridView.Rows[rowIndex];
            gridRow.ReadOnly = false;

            for (int i = 0; i < gridRow.Cells.Count; i++)
            {
                var column = NavigatorDataGridView.Columns[i];
                var dataColumn = _table.Columns[column.DataPropertyName];

                if (dataColumn.ExtendedProperties["IsReference"] as bool? == true)
                {
                    gridRow.Cells[i] = new DataGridViewButtonCell
                    {
                        Value = "Выбрать...",
                        UseColumnTextForButtonValue = false,
                        FlatStyle = FlatStyle.Standard
                    };
                }
            }

            NavigatorDataGridView.CurrentCell = gridRow.Cells[0];
            NavigatorDataGridView.BeginEdit(true);
        }

        private void ResetSorting()
        {
            if (NavigatorDataGridView.DataSource is DataTable table)
            {
                table.DefaultView.Sort = string.Empty;
            }

            foreach (DataGridViewColumn col in NavigatorDataGridView.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private async Task BindTableAsync(Guid typeID)
        {
            var tableData = await _objectService.GetTableData(typeID);

            var table = new DataTable();

            var allAttributes = tableData
                .SelectMany(row => row.Values)
                .Where(attr => attr != null && !string.IsNullOrWhiteSpace(attr.Name))
                .GroupBy(attr => attr.ID)
                .Select(g => g.First())
                .OrderBy(attr => attr.Index)
                .ToList();

            table.ExtendedProperties["typeID"] = typeID;

            foreach (var attr in allAttributes)
            {
                if (!attr.IsVisible)
                    continue;

                if (!attr.IsReference)
                {
                    var column = new DataColumn(attr.Name, attr.ValueType);
                    column.ExtendedProperties["ID"] = attr.ID;
                    column.ExtendedProperties["IsReference"] = false;
                    table.Columns.Add(column);
                }
                else
                {
                    var column = new DataColumn(attr.Name, typeof(string));
                    column.ExtendedProperties["ID"] = attr.ID;
                    column.ExtendedProperties["IsReference"] = true;
                    table.Columns.Add(column);
                }
            }

            foreach (var rowData in tableData)
            {
                var row = table.NewRow();

                foreach (var attr in rowData.Values)
                {
                    if (attr == null || !attr.IsVisible) continue;

                    if (attr.ValueType == typeof(Guid))
                    {
                        var obj = await _objectService.GetObjectByIdAsync(Guid.Parse(attr.Value));
                        row[attr.Name] = obj?.Title ?? "(Не выбран)";
                    }
                    else
                    {
                        row[attr.Name] = attr.Value ?? string.Empty;
                    }
                }

                table.Rows.Add(row);
            }

            _table = table;

            NavigatorDataGridView.AutoGenerateColumns = true;
            NavigatorDataGridView.DataSource = table;
            NavigatorDataGridView.ReadOnly = false;
            NavigatorDataGridView.AllowUserToAddRows = false;
            SetTableReadonlyProperty();
        }

        /// <summary>
        /// Обработчик для выделения всей строки
        /// </summary>
        private void NavigatorDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            NavigatorDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            NavigatorDataGridView.ClearSelection();
            NavigatorDataGridView.Rows[e.RowIndex].Selected = true;
        }

        /// <summary>
        /// Обработчик для выделения отдельной ячейки
        /// </summary>
        private void NavigatorDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                NavigatorDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
                NavigatorDataGridView.ClearSelection();
                NavigatorDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }

            if (!IsEditing)
                return;

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var cell = NavigatorDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (!(cell is DataGridViewButtonCell))
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var dataColumn = _table.Columns[column.DataPropertyName];

            if (dataColumn?.ExtendedProperties["IsReference"] as bool? != true)
                return;

            var attributeId = (Guid)dataColumn.ExtendedProperties["ID"];

            //using (var form = new ReferenceSelectForm(attributeId))
            //{
            //    if (form.ShowDialog() == DialogResult.OK)
            //    {
            //        _newRow[dataColumn.ColumnName] = form.SelectedTitle;

            //        // сохраняем GUID скрыто
            //        _newRow.SetColumnError(dataColumn, form.SelectedId.ToString());

            //        cell.Value = form.SelectedTitle;
            //    }
            //}
        }
    }
}