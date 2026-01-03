using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_DB.Model;

namespace MiniNavigator_Services.Mapper
{
    /// <summary>
    /// Маппер для NavObjectDTO <=> BaseObject
    /// </summary>
    public class ObjectMapper : IMapper<NavObjectDTO, BaseObject>
    {
        public NavObjectDTO ToDTO(BaseObject entity)
        {
            return new NavObjectDTO()
            {
                ID = entity.ID,
                ObjectTypeID = entity.ObjectTypeID,
                Title = nameof(entity),
                Parent = null,
                ParentID = entity.ParentID
            };
        }

        public BaseObject ToEntity(NavObjectDTO dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
