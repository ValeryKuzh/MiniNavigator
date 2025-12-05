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
        public Guid ID { get; set; }

        [Description("Название")]
        [Browsable(true)]
        public string Name { get; set; }
        
        [Description("Тип")]
        [Browsable(true)]
        public string Type { get; set; }
        
        [Browsable(false)]
        public BindingList<NavObjectDTO> Children { get; set; } = new BindingList<NavObjectDTO>();
    }
}
