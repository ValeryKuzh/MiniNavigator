using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.ViewModel
{
    public class ObjectAttributeViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool IsReference { get; set; }
        public Type ValueType { get; set; }
        public string Value { get; set; }
        public Guid? ReferenceID { get; set; }
        public bool IsRequired { get; set; }
    }
}
