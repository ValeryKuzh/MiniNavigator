using System;

namespace MiniNavigator_Services.DTO
{
    /// <summary>
    /// DTO для передачи информации о действиях в системе
    /// </summary>
    public class ObjectActionDTO
    {
        public Guid ID { get; set; }
        public string CommandName { get; set; }
        public string DisplayName { get; set; }
        public Guid ObjectID { get; set; } 
    }
}
