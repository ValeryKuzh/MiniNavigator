using System;
using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    public class ObjectAction
    {
        public Guid ID { get; set; }

        public Guid Base_ID { get; set; }
        public BaseObject Base { get; set; }

        public string Name { get; set; } 

        public ICollection<ObjectType> ObjectTypes { get; set; }
    }
}
