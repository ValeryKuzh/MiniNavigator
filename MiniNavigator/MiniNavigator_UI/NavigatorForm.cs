using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Infralution.Controls.VirtualTree;
using MiniNavigator_Services.Service.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_UI.Mapper;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.ViewModel;

namespace MiniNavigator_UI
{
    public partial class NavigatorForm : Form
    {
        private IObjectService _objectService;
        private IObjectTypeService _objectTypeService;

        private IMapper<NavObjectViewModel, NavObjectDTO> _objectMapper = new ObjectMapper();
        private IMapper<ObjectTypeViewModel, ObjectTypeDTO> _objectTypeMapper = new ObjectTypeMapper();

        private NavObjectViewModel _navObjects = new NavObjectViewModel()
        {
            Title = "Система"
        };

        private BindingList<NavObjectViewModel> _data = new BindingList<NavObjectViewModel>();

        public NavigatorForm()
        {
            InitializeComponent();
            NavigatorDataGridView.DataSource = _data;

            //InitRoot();
            BindTree(); 

            NavigatorVirtualTree.DataSource = _navObjects;
        }

        private void LoadTree()
        {
            _navObjects = _objectMapper.ToViewModel(_objectService.GetTreeOfObjects());
            NavigatorVirtualTree.DataSource = _navObjects;
        }

        private void BindTree()
        {
            // создаём биндинг для NavObjectViewModel
            var binding = new ObjectRowBinding(typeof(NavObjectViewModel));

            // указываем, что Children — это дочерние элементы
            binding.ChildProperty = "Children";

            var nameBinding = new ObjectCellBinding
            {
                Column = NavigatorVirtualTree.Columns[0],
                Field = "Name"
            };

            binding.CellBindings.Add(nameBinding);

            // Регистрируем биндинг
            NavigatorVirtualTree.RowBindings.Add(binding);
        }

        private async void InitRoot()
        {
            List<ObjectTypeDTO> types = await _objectTypeService.GetTypesAsync();

            foreach (var type in types)
            {
                _navObjects.Children.Add(_objectTypeMapper.ToViewModel(type));
            }
            //_navObjects.Children.Add(new NavObjectDTO
            //{
            //    Name = "Пользователи",
            //    Type = "User",
            //    Children =
            //    {
            //        new NavObjectDTO { Name = "Модуль 1", Type = "Module" },
            //        new NavObjectDTO
            //        {
            //            Name = "Модуль 2",
            //            Type = "Module",
            //            Children =
            //            {
            //                new NavObjectDTO { Name = "Объект A", Type = "Entity" },
            //                new NavObjectDTO { Name = "Объект B", Type = "Entity" }
            //            }
            //        }
            //    }
            //});
            //_navObjects.Children.Add(new NavObjectDTO
            //{
            //    Name = "Роли",
            //    Type = "Role",
            //    Children =
            //    {
            //        new NavObjectDTO { Name = "Модуль 1", Type = "Module" },
            //        new NavObjectDTO
            //        {
            //            Name = "Модуль 2",
            //            Type = "Module",
            //            Children =
            //            {
            //                new NavObjectDTO { Name = "Объект A", Type = "Entity" },
            //                new NavObjectDTO { Name = "Объект B", Type = "Entity" }
            //            }
            //        }
            //    }
            //});
        }
        private async void NavigatorVirtualTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (NavigatorVirtualTree.SelectedRow?.Item is NavObjectDTO navObjectDto)
                {

                    var actions = _objectService.GetActionsForObject(navObjectDto.ID).Result;

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
