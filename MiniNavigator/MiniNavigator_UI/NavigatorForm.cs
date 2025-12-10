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
using System.Linq;
using System.Windows.Forms;

namespace MiniNavigator_UI
{
    public partial class NavigatorForm : Form
    {
        private IObjectService _objectService;
        private IObjectTypeService _objectTypeService;

        private IMapper<NavObjectViewModel, NavObjectDTO> _objectMapper = new ObjectMapper();
        private IMapper<ObjectTypeViewModel, ObjectTypeDTO> _objectTypeMapper = new ObjectTypeMapper();

        private NavObjectViewModel _treeOfObjects = new NavObjectViewModel();

        private BindingList<NavObjectViewModel> _data = new BindingList<NavObjectViewModel>();

        public NavigatorForm(IObjectService objectService, IObjectTypeService objectTypeService)
        {
            _objectService = objectService;
            _objectTypeService = objectTypeService;

            InitializeComponent();

            this.Load += NavigatorForm_Load;

            BindTree();

            NavigatorVirtualTree.DataSource = _treeOfObjects;
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

            // Регистрируем биндинг
            NavigatorVirtualTree.RowBindings.Add(binding);
        }

        private async void NavigatorForm_Load(object sender, EventArgs e)
        {

            InitRoot();
        }

        private async void InitRoot()
        {
            var tree = ConvertTreeToViewModels(await _objectService.GetTreeOfObjectsAsync());
            _treeOfObjects = tree;
        }

        public NavObjectViewModel ConvertTreeToViewModels(NavObjectDTO root)
        {
            var result = _objectMapper.ToViewModel(root);

            if (root.Children != null && root.Children.Any())
            {
                result.Children = new BindingList<NavObjectViewModel>();
                foreach (var child in root.Children)
                {
                    result.Children.Add(ConvertTreeToViewModels(child));
                }
            }
            
            return result;
        }

        private async void NavigatorVirtualTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (NavigatorVirtualTree.SelectedRow?.Item is NavObjectDTO navObjectDto)
                {

                    var actions = await _objectService.GetActionsForObject(navObjectDto.ID);

                    ContextMenuStrip menu = new ContextMenuStrip();

                    foreach (var action in actions)
                    {
                        var item = new ToolStripMenuItem(action.CommandName);
                        item.Tag = action;
                        item.Click += ActionMenuItem_Click;
                        menu.Items.Add(item);
                    }

                    menu.Show(NavigatorVirtualTree, e.Location);
                }
            }
        }

        private void ActionMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
