using System;
using System.Collections.Generic;

namespace MiniNavigator_Services.DTO
{
    /// <summary>
    /// DTO для передачи объектов, которые отображаются в дереве типов
    /// </summary>
    public class NavObjectDTO
    {
        public Guid ID { get; set; }
        public Guid? ObjectTypeID { get; set; }
        public string Title { get; set; }
        public Guid? ParentID { get; set; }
        public NavObjectDTO Parent { get; set; }
        public List<NavObjectDTO> Children { get; set; } = new List<NavObjectDTO>();
    }
}
