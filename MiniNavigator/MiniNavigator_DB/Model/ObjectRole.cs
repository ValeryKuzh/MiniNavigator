using System;

namespace MiniNavigator_DB.Model
{
    public class ObjectRole
    {
        public Guid ID { get; set; }

        public Guid Base_ID { get; set; }
        public BaseObject Base { get; set; }
    }
}
