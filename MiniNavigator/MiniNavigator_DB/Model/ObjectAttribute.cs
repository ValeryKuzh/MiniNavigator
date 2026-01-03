using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Атрибут объекта
    /// </summary>
    public class ObjectAttribute
    {
        public Guid ID { get; set; }
        public Guid Base_ID { get; set; }
        [ForeignKey("Base_ID")]
        public BaseObject Base { get; set; }

        public string Name { get; set; }
        
        public bool IsReference { get; set; }
        public Guid? ReferenceObjectTypeID { get; set; }

        [ForeignKey("ReferenceObjectTypeID")]
        public ObjectType ReferenceObjectType { get; set; }


        public string ValueType { get; set; }

        [NotMapped]
        public Type ValType
        {
            get => ValueType == null ? null : Type.GetType(ValueType);
            set => ValueType = value?.AssemblyQualifiedName;
        }

        public ICollection<ObjectTypeAttribute> ObjectTypes { get; set; }
    }
}
