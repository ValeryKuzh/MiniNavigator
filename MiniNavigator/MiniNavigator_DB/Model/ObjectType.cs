using System;
using System.Collections.Generic;

namespace DataBase.Model
{
    public class ObjectType
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<ObjectAction> Actions { get; set; }
        public List<ObjectAttribute> Attributes { get; set; }
        public ICollection<Object> Objects { get; set; }
    }
}
