using System;

namespace DataBase.Model
{
    public class ObjectAttribute
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public ValueType ValueType { get; set; }
        public string Value { get; set; }
    }
}
