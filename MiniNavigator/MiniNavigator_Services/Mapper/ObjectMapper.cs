using MiniNavigator_Services.DTO;
using MiniNavigator_DB.Model;

namespace MiniNavigator_Services.Mapper
{
    public class ObjectMapper : IMapper<NavObjectDTO, BaseObject>
    {
        public NavObjectDTO ToDto(BaseObject entity)
        {
            throw new System.NotImplementedException();
        }

        public BaseObject ToEntity(NavObjectDTO dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
