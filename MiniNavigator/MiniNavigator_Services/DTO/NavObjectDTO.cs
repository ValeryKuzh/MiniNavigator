using System;
using System.ComponentModel;

namespace MiniNavigator_Services.DTO
{
    public class NavObjectDTO
    {
        public Guid ID { get; set; }

        /// <summary>
        /// Название объекта
        /// </summary>
        [Description("Название")]
        [Browsable(true)]
        public virtual string Title { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        [Description("Тип")]
        public ObjectTypeDTO Type { get; set; }
        
        [Description("Дочерние узлы")]
        public BindingList<NavObjectDTO> Children { get; set; } = new BindingList<NavObjectDTO>();
    }
}
