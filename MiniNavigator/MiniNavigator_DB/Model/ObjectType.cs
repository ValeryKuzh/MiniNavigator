using System;
using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    public class ObjectType
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public ICollection<ObjectAction> Actions { get; set; }
        public ICollection<ObjectAttribute> Attributes { get; set; }
        
        public ICollection<BaseObject> Objects { get; set; }
    }
}
