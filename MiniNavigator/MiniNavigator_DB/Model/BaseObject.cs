using System;

namespace MiniNavigator_DB.Model
{
    public class BaseObject
    {
        public Guid ID { get; set; }
        public Guid ObjectTypeID { get; set; }
        public ObjectType ObjectType { get; set; }
    }
}
