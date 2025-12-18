using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    public class ObjectTypeAttribute
    {
        public Guid ObjectTypeID { get; set; }
        
        [ForeignKey("ObjectTypeID")]
        public ObjectType ObjectType { get; set; }

        public Guid AttributeID { get; set; }
        
        [ForeignKey("AttributeID")]
        public ObjectAttribute Attribute { get; set; }
        
        public bool IsRequired { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }
    }
}
