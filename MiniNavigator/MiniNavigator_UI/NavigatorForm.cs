using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infralution.Controls.VirtualTree;
using MiniNavigator_UI.DTO;


namespace MiniNavigator_UI
{
    public partial class NavigatorForm : Form
    {
        private IObjectService _objectService;
        private NavObjectDTO _navObjects = new NavObjectDTO()
        {
            Name = "Система",
            Type = "System"
        };

        private BindingList<NavObjectDTO> _data = new BindingList<NavObjectDTO>();
        public NavigatorForm()
        {
            InitializeComponent();
            NavigatorDataGridView.DataSource = _data;

            InitRoot();
            BindTree(); 

            NavigatorVirtualTree.DataSource = _navObjects;
        }

        private void LoadTree()
        {
            _navObjects = _objectService.GetTreeOfObjects();
            NavigatorVirtualTree.DataSource = _navObjects;
        }
        private void BindTree()
        {
            // создаём биндинг для NavObjectDTO
            var binding = new ObjectRowBinding(typeof(NavObjectDTO));

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

        private void InitRoot()
        {
            _navObjects.Children.Add(new NavObjectDTO
            {
                Name = "Пользователи",
                Type = "User",
                Children =
                {
                    new NavObjectDTO { Name = "Модуль 1", Type = "Module" },
                    new NavObjectDTO
                    {
                        Name = "Модуль 2",
                        Type = "Module",
                        Children =
                        {
                            new NavObjectDTO { Name = "Объект A", Type = "Entity" },
                            new NavObjectDTO { Name = "Объект B", Type = "Entity" }
                        }
                    }
                }
            });
            _navObjects.Children.Add(new NavObjectDTO
            {
                Name = "Роли",
                Type = "Role",
                Children =
                {
                    new NavObjectDTO { Name = "Модуль 1", Type = "Module" },
                    new NavObjectDTO
                    {
                        Name = "Модуль 2",
                        Type = "Module",
                        Children =
                        {
                            new NavObjectDTO { Name = "Объект A", Type = "Entity" },
                            new NavObjectDTO { Name = "Объект B", Type = "Entity" }
                        }
                    }
                }
            });
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
