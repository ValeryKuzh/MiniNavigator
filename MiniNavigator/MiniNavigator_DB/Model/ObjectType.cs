    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Тип объекта системы
    /// </summary>
    public class ObjectType
    {
        public Guid ID { get; set; }
        public Guid Base_ID { get; set; }
        [ForeignKey("Base_ID")]
        public BaseObject Base { get; set; }

        public string Name { get; set; }
        public bool IsVisible { get; set; }

        public ICollection<ObjectAction> Actions { get; set; }
        public ICollection<ObjectAttribute> Attributes { get; set; }
    }
}