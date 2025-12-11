using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    public class ObjectUser
    {
        public Guid ID { get; set; }

        public Guid Base_ID { get; set; }
        [ForeignKey("BaseID")]
        public BaseObject Base { get; set; }

        public Guid RoleID { get; set; }
        public ObjectRole Role { get; set; }
    }
}
