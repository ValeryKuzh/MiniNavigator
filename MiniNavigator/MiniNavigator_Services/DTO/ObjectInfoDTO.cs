using System;

namespace MiniNavigator_Services.DTO
{
    /// <summary>
    /// DTO для передачи Title объекта
    /// </summary>
    public class ObjectInfoDTO
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
    }
}
