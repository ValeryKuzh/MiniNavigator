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
        private BindingList<NavObjectDTO> _navObjects = new BindingList<NavObjectDTO>();
        public NavigatorForm()
        {
            InitializeComponent();
            this.NavigatorVirtualTree.DataSource = _navObjects;
            InitRot();
        }

        private void InitRot()
        {
            _navObjects.Add(new NavObjectDTO
            {
                Name = "Система",
                Type = "Root",
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
