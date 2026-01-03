using System;

namespace MiniNavigator_Services.DTO
{
    /// <summary>
    /// DTO для передачи информации об атрибуте с его значением
    /// </summary>
    public class ObjectAttributeDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool IsReference { get; set; }
        public Guid? ReferenceObjectTypeID { get; set; }
        public Type ValueType { get; set; }
        public string Value { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTitle { get; set; }
        public int Index { get; set; }
    }
}
