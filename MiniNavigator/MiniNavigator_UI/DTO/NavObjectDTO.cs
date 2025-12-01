using Infralution.Controls.VirtualTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.DTO
{
    public class NavObjectDTO
    {
        [Description("Название")]
        public string Name { get; set; }
        [Description("Тип")]
        public string Type { get; set; }
        public List<NavObjectDTO> Children { get; set; } = new List<NavObjectDTO>();
    }
}
