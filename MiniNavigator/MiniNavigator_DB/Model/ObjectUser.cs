using System;

namespace MiniNavigator_DB.Model
{
    public class ObjectUser
    {
        public Guid ID { get; set; }
        public BaseObject Base { get; set; }

        public Guid RoleID { get; set; }
        public ObjectRole Role { get; set; }
    }
}
