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
                ID = entity.ID,
                Title = entity.Name,
                ObjectID = entity.Base_ID,
                IsVisible = entity.IsVisible
            };
        }

        public ObjectType ToEntity(ObjectTypeDTO dto)
        {
            return new ObjectType()
            {
                ID = dto.ID,
                Name = dto.Title
            };
        }
    }
}
