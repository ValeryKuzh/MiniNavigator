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
        private NavObjectDTO _navObjects = new NavObjectDTO()
        {
            Name = "Система",
            Type = "System"
        };
        public NavigatorForm()
        {
            InitializeComponent();

            InitRoot();
            BindTree();

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
                Name = "Файлы",
                Type = "File",
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
        }
    }
}
