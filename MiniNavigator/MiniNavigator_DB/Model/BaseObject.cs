using System;
using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Объект системы
    /// </summary>
    public class BaseObject
    {
        public Guid ID { get; set; }
        public Guid? ObjectTypeID { get; set; }
        public BaseObject ObjectType { get; set; }
        public Guid? ParentID { get; set; }
        public BaseObject Parent { get; set; }
        public ICollection<BaseObject> Children { get; set; }
    }
}
