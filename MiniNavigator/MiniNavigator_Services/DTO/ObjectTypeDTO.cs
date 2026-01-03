using System;

namespace MiniNavigator_Services.DTO
{
    /// <summary>
    /// DTO для передачи типов объектов в дерево
    /// </summary>
    public class ObjectTypeDTO
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public Guid ObjectID { get; set; }
        public bool IsVisible { get; set; }
    }
}
