using MiniNavigator_DB.Model;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Mapper
{
    public class AttributeMapper : IMapper<ObjectAttributeDTO, ObjectAttributeValue>
    {
        public ObjectAttributeDTO ToDTO(ObjectAttributeValue entity)
        {
            return new ObjectAttributeDTO()
            {
                ID = entity.Attribute.ID,
                Name = entity.Attribute.Name,
                ValueType = entity.Attribute.ValType,
                Value = entity.Value
            };
        }

        public ObjectAttributeValue ToEntity(ObjectAttributeDTO DTO)
        {
            throw new System.NotImplementedException();
        }
    }
}
