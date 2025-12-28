using MiniNavigator_DB.Model;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Mapper
{
    public class ObjectActionMapper : IMapper<ObjectActionDTO, ObjectAction>
    {
        public ObjectActionDTO ToDTO(ObjectAction entity)
        {
            return new ObjectActionDTO()
            {
                ID = entity.ID,
                CommandName = entity.Name,
                DisplayName = entity.DisplayName,
                ObjectID = entity.Base_ID
            };
        }

        public ObjectAction ToEntity(ObjectActionDTO DTO)
        {
            throw new NotImplementedException();
        }
    }
}
