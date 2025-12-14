using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    public class ObjectAction
    {
        public Guid ID { get; set; }

        public Guid Base_ID { get; set; }
        [ForeignKey("Base_ID")]
        public BaseObject Base { get; set; }

        public string Name { get; set; } 

        public ICollection<ObjectType> ObjectTypes { get; set; }
    }
}
