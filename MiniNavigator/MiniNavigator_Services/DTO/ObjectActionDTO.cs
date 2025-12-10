using System;

namespace MiniNavigator_Services.DTO
{
    public class ObjectActionDTO
    {
        public Guid ID { get; set; }
        public string CommandName { get; set; }

        public Guid ObjectID { get; set; } 
    }
}
