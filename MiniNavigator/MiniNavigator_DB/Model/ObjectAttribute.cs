using System;
using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    public class ObjectAttribute
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }

        public ICollection<ObjectType> ObjectTypes { get; set; }
    }
}
