using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Допустимые атрибуты для объекта
    /// </summary>
    public class ObjectAttribute
    {
        public Guid ID { get; set; }
        public Guid Base_ID { get; set; }
        [ForeignKey("Base_ID")]
        public BaseObject Base { get; set; }

        public string Name { get; set; }
        
        public bool IsReference { get; set; }
        public string ValueType { get; set; }

        [NotMapped]
        public Type ValType
        {
            get => ValueType == null ? null : Type.GetType(ValueType);
            set => ValueType = value?.AssemblyQualifiedName;
        }

        public ICollection<ObjectType> ObjectTypes { get; set; }
    }
}
