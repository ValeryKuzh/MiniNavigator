using Infralution.Controls.VirtualTree;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Service.Interface;
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
using System.Windows.Forms.VisualStyles;

namespace MiniNavigator_UI
{
    public partial class NavigatorForm : Form
    {
        private readonly IObjectService _objectService;
        private readonly IObjectTypeService _objectTypeService;
        private readonly IValidationService _validationService;

        private readonly IMapper<NavObjectViewModel, NavObjectDTO> _objectMapper;

        private NavObjectViewModel _treeOfObjects = new NavObjectViewModel();

        private BindingList<DynamicObjectRow> _rows;
        private List<ObjectAttributeViewModel> _attributes;
        private Guid _currentTypeId;


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
            NavigatorDataGridView.CellPainting += NavigatorDataGridView_CellPainting;

            this.Load += NavigatorForm_Load;
        }

        private void NavigatorDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;

            if (attr == null || !attr.IsReference)
                return;

            var row = _rows[e.RowIndex];

            if (row != _editingRow || !IsEditing)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);
                return;
            }

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            var text = row.Attributes[attr.ID].Value;
            if (string.IsNullOrWhiteSpace(text))
                text = "Выбрать...";

            var rect = new Rectangle(
                e.CellBounds.X + 2,
                e.CellBounds.Y + 2,
                e.CellBounds.Width - 4,
                e.CellBounds.Height - 4
            );

            ButtonRenderer.DrawButton(
                e.Graphics,
                rect,
                text,
                NavigatorDataGridView.Font,
                false,
                PushButtonState.Normal
            );
        }

        private void NavigatorDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!IsEditing || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];

            var attr = column.Tag as ObjectAttributeViewModel;
            if (attr == null)
                return;

            // Ссылочные атрибуты не валидируем
            if (attr.IsReference)
                return;

            var value = e.FormattedValue?.ToString();
            if (string.IsNullOrWhiteSpace(value))
                return;

            string error;
            bool valid = _validationService.ValidateSingleValue(
                value,
                attr.ValueType,
                out error
            );

            if (!valid)
            {
                e.Cancel = true;
                MessageBox.Show(
                    error,
                    "Ошибка ввода",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
            var actions = await _objectService.GetActionsForTypeObject(navObjectViewModel.ID);

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
            if (NavigatorVirtualTree.SelectedRow?.Item is NavObjectViewModel && _rows != null)
            {
                ResetSorting();
                SetTableReadonlyProperty();

                AddNewRow();

                IsEditing = true;
                InitializeCreateNewObject();
                ShowControls();
            }
        }


        private void AddNewRow()
        {
            var row = new DynamicObjectRow();

            foreach (var attr in _attributes)
            {
                row.Attributes[attr.ID] = new ObjectAttributeViewModel
                {
                    ID = attr.ID,
                    Name = attr.Name,
                    ValueType = attr.ValueType,
                    IsReference = attr.IsReference,
                    IsRequired = attr.IsRequired,
                    Value = null
                };
            }

            _rows.Add(row);
            _editingRow = row;

            int lastRowIndex = NavigatorDataGridView.Rows.Count - 1;
            if (lastRowIndex >= 0)
            {
                NavigatorDataGridView.CurrentCell =
                    NavigatorDataGridView.Rows[lastRowIndex].Cells[0];
            }

            NavigatorDataGridView.BeginEdit(true);
        }

        private void ResetSorting()
        {
            foreach (DataGridViewColumn col in NavigatorDataGridView.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }


        /// <summary>
        /// Биндинг объектов системы на DataGridView
        /// </summary>
        /// <param name="typeId">Тип объектов</param>
        private async Task BindTableAsync(Guid typeId)
        {
            _currentTypeId = typeId;

            var tableData = await _objectService.GetTableData(typeId);

            _attributes = ExtractAttributes(tableData);

            ConfigureGrid();

            CreateColumns(_attributes);

            _rows = await CreateRowsAsync(tableData);

            BindGrid(_rows);

            IsEditing = false;
        }

        /// <summary>
        /// Маппит Полученные атрибуты из сервиса на ViewModel объекты
        /// </summary>
        /// <param name="tableData">Список объектов из сервиса</param>
        /// <returns>Список ViewModel объектов</returns>
        private List<ObjectAttributeViewModel> ExtractAttributes(List<Dictionary<Guid, ObjectAttributeDTO>> tableData)
        {
            return tableData
                .SelectMany(dict => dict.Values)
                .Where(a => a != null && a.IsVisible && !string.IsNullOrWhiteSpace(a.Name))
                .GroupBy(a => a.ID)
                .Select(g => g.First())
                .OrderBy(a => a.Index)
                .Select(a => new ObjectAttributeViewModel
                {
                    ID = a.ID,
                    Name = a.Name,
                    ValueType = a.ValueType,
                    IsReference = a.IsReference,
                    IsRequired = a.IsRequired
                })
                .ToList();
        }

        /// <summary>
        /// Конфигурация DataGridView
        /// </summary>
        private void ConfigureGrid()
        {
            NavigatorDataGridView.AutoGenerateColumns = false;
            NavigatorDataGridView.Columns.Clear();
            NavigatorDataGridView.AllowUserToAddRows = false;
            NavigatorDataGridView.ReadOnly = false;

            NavigatorDataGridView.VirtualMode = true;
            NavigatorDataGridView.CellValueNeeded += NavigatorDataGridView_CellValueNeeded;
            NavigatorDataGridView.CellValuePushed += NavigatorDataGridView_CellValuePushed;
        }


        /// <summary>
        /// Создание колонок в таблице
        /// </summary>
        /// <param name="attributes">ViewModel объекты атрибутов</param>
        private void CreateColumns(List<ObjectAttributeViewModel> attributes)
        {
            foreach (var attr in attributes)
            {
                var column = CreateColumn(attr);
                NavigatorDataGridView.Columns.Add(column);
            }
        }

        /// <summary>
        /// Создание одной колонки 
        /// </summary>
        /// <param name="attr">ViewModel объект атрибута</param>
        /// <returns>Колонка DataGridView</returns>
        private DataGridViewColumn CreateColumn(ObjectAttributeViewModel attr)
        {
            DataGridViewColumn column;

            if (attr.ValueType == typeof(bool))
            {
                column = new DataGridViewCheckBoxColumn();
            }
            else
            {
                column = new DataGridViewTextBoxColumn();
            }

            column.Name = attr.Name;
            column.HeaderText = attr.Name;
            column.Tag = attr;
            column.ReadOnly = false;

            return column;
        }

        /// <summary>
        /// Создание строк
        /// </summary>
        /// <param name="tableData">Данные об объектах из сервиса</param>
        /// <returns>Структура для отображения на DataGridView</returns>
        private async Task<BindingList<DynamicObjectRow>> CreateRowsAsync(List<Dictionary<Guid, ObjectAttributeDTO>> tableData)
        {
            var rows = new BindingList<DynamicObjectRow>();

            foreach (var rowDict in tableData)
            {
                var row = await CreateRowAsync(rowDict);
                rows.Add(row);
            }

            return rows;
        }

        /// <summary>
        /// Создание строки
        /// </summary>
        /// <param name="rowDict">Данные о строке</param>
        /// <returns>Строка для DataGridView</returns>
        private async Task<DynamicObjectRow> CreateRowAsync(Dictionary<Guid, ObjectAttributeDTO> rowDict)
        {
            var row = new DynamicObjectRow();

            var idAttr = rowDict.Values.FirstOrDefault(a => a.Name == "ID");
            if (idAttr != null && Guid.TryParse(idAttr.Value, out var objectId))
            {
                row.ObjectId = objectId;
            }

            foreach (var attr in rowDict.Values)
            {
                if (attr == null || !attr.IsVisible)
                    continue;

                var value = await ResolveAttributeValueAsync(attr);

                row.Attributes[attr.ID] = new ObjectAttributeViewModel
                {
                    ID = attr.ID,
                    Name = attr.Name,
                    ValueType = attr.ValueType,
                    IsReference = attr.IsReference,
                    IsRequired = attr.IsRequired,
                    Value = value
                };
            }

            return row;
        }

        /// <summary>
        /// Обработчик для отображения значения в ячейке
        /// </summary>
        private void NavigatorDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _rows.Count)
                return;

            var row = _rows[e.RowIndex];
            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;
            if (attr == null)
                return;

            if (row.Attributes.TryGetValue(attr.ID, out var avm))
                e.Value = avm.Value;
        }

        /// <summary>
        /// Обработчик для получения значения из ячейки
        /// </summary>
        private void NavigatorDataGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (!_isEditing)
                return;

            var row = _rows[e.RowIndex];

            if (row != _editingRow)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;

            if (attr == null)
                return;

            if (attr.IsReference)
                return;

            if (row.Attributes.TryGetValue(attr.ID, out var vm))
                vm.Value = e.Value?.ToString();
        }


        /// <summary>
        /// Определение значения для отображения атрибута 
        /// </summary>
        /// <param name="attr">Атрибут</param>
        /// <returns>Значение</returns>
        private async Task<string> ResolveAttributeValueAsync(ObjectAttributeDTO attr)
        {
            if (!attr.IsReference || attr.Value == null)
                return attr.Value ?? string.Empty;

            var obj = await _objectService.GetObjectInfoByIdAsync(Guid.Parse(attr.Value.ToString()));

            return obj?.Title ?? "(Не выбран)";
        }

        /// <summary>
        /// Биндинг DataGridView 
        /// </summary>
        /// <param name="rows">Струтура для биндинга</param>
        private void BindGrid(BindingList<DynamicObjectRow> rows)
        {
            NavigatorDataGridView.DataSource = rows;
        }

        private async void NavigatorDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            NavigatorDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            NavigatorDataGridView.ClearSelection();
            NavigatorDataGridView.Rows[e.RowIndex].Selected = true;

            if (e.Button != MouseButtons.Right)
                return;

            // Получаем объект из строки
            var row = _rows[e.RowIndex];
            if (row == null)
                return;

            // Показываем контекстное меню
            await ShowContextMenuForRowAsync(row, NavigatorDataGridView.PointToClient(Cursor.Position));
        }

        private async Task ShowContextMenuForRowAsync(DynamicObjectRow row, Point location)
        {
            var objectId = row.ObjectId;
            var actions = await _objectService.GetActionsForTypeObject(objectId);

            if (actions == null || actions.Count == 0)
                return;

            ContextMenuStrip menu = new ContextMenuStrip();

            foreach (var action in actions)
            {
                var item = new ToolStripMenuItem(action.CommandName) { Tag = action };
                menu.Items.Add(item);
                item.Click += ContextMenuItem_Click;
            }

            menu.Show(NavigatorDataGridView, location);
        }

        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is ObjectActionDTO action)
            {
                MessageBox.Show($"Вызвано действие: {action.CommandName}");
            }
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

            if (!IsEditing || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = _rows[e.RowIndex];
            if (row != _editingRow)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;

            if (attr == null || !attr.IsReference)
                return;

            if (attr.IsReference && row == _editingRow && IsEditing)
            {
                using (var form = new ReferenceSelectForm(attr.ID, _objectService))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var vm = row.Attributes[attr.ID];

                        vm.Value = form.SelectedTitle;
                        vm.ReferenceID = form.SelectedID;

                        NavigatorDataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                    }
                }
            }
        }
    }
}