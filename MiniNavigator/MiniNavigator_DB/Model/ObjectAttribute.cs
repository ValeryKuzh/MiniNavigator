using System;
using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Допустимые атрибуты для объекта
    /// </summary>
    public class ObjectAttribute
    {
        public Guid ID { get; set; }
        public BaseObject Base { get; set; }

        public string Name { get; set; }
        public string ValueType { get; set; }

        public ICollection<ObjectType> ObjectTypes { get; set; }
    }
}
