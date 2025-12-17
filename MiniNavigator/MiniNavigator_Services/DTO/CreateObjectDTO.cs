using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.DTO
{
    public class CreateObjectDTO
    {
        public Guid ID { get; set; }
        public Dictionary<Guid, ObjectAttributeDTO> Attributes { get; set; } = new Dictionary<Guid, ObjectAttributeDTO>();
    }
}
