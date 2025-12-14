using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Значение атрибута у объекта
    /// </summary>
    public class ObjectAttributeValue
    {
        public Guid ObjectID { get; set; } 
        public BaseObject Object { get; set; }

        public Guid AttributeID { get; set; }
        public ObjectAttribute Attribute { get; set; }
        
        // public int Index { get; set; } // возможно создание мультизначного атрибута 

        public string Value { get; set; }
    }
}
