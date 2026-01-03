using System;
using System.Collections.Generic;

namespace MiniNavigator_Services.DTO
{
    /// <summary>
    /// DTO для передачи атрибутов объекта с их значениями
    /// </summary>
    public class ObjectDTO
    {
        public Guid ID { get; set; }
        public Dictionary<Guid, ObjectAttributeDTO> Attributes { get; set; } = new Dictionary<Guid, ObjectAttributeDTO>();
    }
}
