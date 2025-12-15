using Infralution.Controls.VirtualTree;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Service;
using MiniNavigator_Services.Service.Interface;
using MiniNavigator_UI.Mapper;
using MiniNavigator_UI.Mapper.Interface;
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
        private IObjectService _objectService;
        private IObjectTypeService _objectTypeService;

        private IMapper<NavObjectViewModel, NavObjectDTO> _objectMapper;

        private NavObjectViewModel _treeOfObjects = new NavObjectViewModel();

        private DataTable _table;
        private BindingList<ObjectDynamicAtributeViewModel> _objectsData;

        public NavigatorForm(
            IObjectService objectService,
            IObjectTypeService objectTypeService,
            IMapper<NavObjectViewModel, NavObjectDTO> objectMapper)
        {
            _objectService = objectService;
            _objectTypeService = objectTypeService;

            _objectMapper = objectMapper;

            InitializeComponent();

            NavigatorDataGridView.RowHeaderMouseClick += NotesGridView_RowHeaderMouseClick;
            NavigatorDataGridView.CellClick += NotesGridView_CellClick;

            this.Load += NavigatorForm_Load;
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
                InitializeCreateNewObject();
                ShowControls();
                //_objectService.AddNewObject();
            }
        }

        private async Task BindTableAsync(Guid typeID)
        {
            var tableData = await _objectService.GetTableData(typeID);

            if (tableData == null || tableData.Count == 0)
            {
                NavigatorDataGridView.DataSource = null;
                return;
            }

            var table = new DataTable();

            var allAttributes = tableData
                .SelectMany(row => row.Values)
                .Where(attr => attr != null && !string.IsNullOrWhiteSpace(attr.Name))
                .GroupBy(attr => attr.ID)
                .Select(g => g.First())
                .OrderBy(attr => attr.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var attr in allAttributes)
            {
                table.Columns.Add(attr.Name, attr.ValueType);
            }

            foreach (var rowData in tableData)
            {
                var row = table.NewRow();

                foreach (var attr in rowData.Values)
                {
                    if (attr == null) continue;
                    row[attr.Name] = attr.Value ?? string.Empty;
                }

                table.Rows.Add(row);
            }

            _table = table;
            
            NavigatorDataGridView.AutoGenerateColumns = true;
            NavigatorDataGridView.DataSource = table;
        }

        /// <summary>
        /// Обработчик для выделения всей строки
        /// </summary>
        private void NotesGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            NavigatorDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            NavigatorDataGridView.ClearSelection();
            NavigatorDataGridView.Rows[e.RowIndex].Selected = true;
        }

        /// <summary>
        /// Обработчик для выделения отдельной ячейки
        /// </summary>
        private void NotesGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                NavigatorDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
                NavigatorDataGridView.ClearSelection();
                NavigatorDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }
    }
}