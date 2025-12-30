using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    public class ObjectUser
    {
        public Guid ID { get; set; }

        public Guid Base_ID { get; set; }
        [ForeignKey("Base_ID")]
        public BaseObject Base { get; set; }

        public Guid? RoleID { get; set; }
        [ForeignKey("RoleID")]
        public ObjectRole Role { get; set; }
    }
}
