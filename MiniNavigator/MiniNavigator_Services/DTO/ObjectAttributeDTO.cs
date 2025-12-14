using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.DTO
{
    public class ObjectAttributeDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Type ValueType { get; set; }
        public string Value { get; set; }
    }
}
