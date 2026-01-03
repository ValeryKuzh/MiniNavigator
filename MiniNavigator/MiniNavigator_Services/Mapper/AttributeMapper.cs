using MiniNavigator_DB.Model;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;

namespace MiniNavigator_Services.Mapper
{
    /// <summary>
    /// Маппер для ObjectAttributeDTO <=> ObjectAttributeValue
    /// </summary>
    public class AttributeMapper : IMapper<ObjectAttributeDTO, ObjectAttributeValue>
    {
        public ObjectAttributeDTO ToDTO(ObjectAttributeValue entity)
        {
            return new ObjectAttributeDTO()
            {
                ID = entity.Attribute.ID,
                Name = entity.Attribute.Name,
                IsReference = entity.Attribute.IsReference,
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
