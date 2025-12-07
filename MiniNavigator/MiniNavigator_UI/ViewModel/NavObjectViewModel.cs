using System;
using System.ComponentModel;

namespace MiniNavigator_UI.ViewModel
{
    public class NavObjectViewModel
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
        public ObjectTypeViewModel Type { get; set; }

        [Description("Дочерние узлы")]
        public BindingList<NavObjectViewModel> Children { get; set; } = new BindingList<NavObjectViewModel>();
    }
}
