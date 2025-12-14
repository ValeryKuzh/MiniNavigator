using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.DTO
{
    public class AttributeValuePair
    {
        public ObjectAttribute Attribute { get; set; }
        public ObjectAttributeValue Value { get; set; }
    }
}
