using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.ViewModel
{
    public class ObjectDynamicAtributeViewModel
    {
        public Guid ID { get; set; } // ID объекта
        public Dictionary<Guid, AttributeViewModel> Attributes { get; set; } = new Dictionary<Guid, AttributeViewModel>(); 
    }
}