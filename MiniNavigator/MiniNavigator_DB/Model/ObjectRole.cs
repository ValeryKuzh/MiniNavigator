using System;

namespace MiniNavigator_DB.Model
{
    public class ObjectRole
    {
        public Guid ID { get; set; }
        public BaseObject Base { get; set; }

        public string RoleName { get; set; }
    }
}
