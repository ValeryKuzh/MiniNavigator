using MiniNavigator_DB.Model;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;

namespace MiniNavigator_Services.Mapper
{
    public class ObjectTypeMapper : IMapper<ObjectTypeDTO, ObjectType>
    {
        public ObjectTypeDTO ToDTO(ObjectType entity)
        {
            return new ObjectTypeDTO()
            {
                Title = entity.Name
            };
        }

        public ObjectType ToEntity(ObjectTypeDTO dto)
        {
            return new ObjectType()
            {
                Name = dto.Title
            };
        }
    }
}
