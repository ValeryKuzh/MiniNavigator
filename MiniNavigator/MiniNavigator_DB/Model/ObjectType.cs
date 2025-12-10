using System;
using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Тип объекта системы
    /// </summary>
    public class ObjectType
    {
        public Guid ID { get; set; }
        public Guid Base_ID { get; set; }
        public BaseObject Base { get; set; }

        public string Name { get; set; }

        public ICollection<ObjectAction> Actions { get; set; }
        public ICollection<ObjectAttribute> Attributes { get; set; }
    }
}