using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository;
using MiniNavigator_Services.Mapper;
using MiniNavigator_UI;
using MiniNavigator_UI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    public class ObjectService : IObjectService
    {
        private readonly IMapper<NavObjectDTO, BaseObject> _objectMapper;
        private readonly IMapper<ObjectActionDTO, ObjectAction> _actionMapper;

        private readonly IRepository<BaseObject> _objectRepository;

        public ObjectService(IMapper<NavObjectDTO, BaseObject> objectMapper, IMapper<ObjectActionDTO, ObjectAction> actionMapper, IRepository<BaseObject> objectRepository)
        {
            _objectMapper = objectMapper;
            _actionMapper = actionMapper;
            _objectRepository = objectRepository;
        }

        public async Task<List<ObjectActionDTO>> GetActionsForObject(Guid ID)
        {
            var obj = _objectRepository.GetByIdAsync(ID);
            var actionsEntity = obj.Result.ObjectType.Actions.ToList();
            var actionsDTO = new List<ObjectActionDTO>();
            foreach(var actionEntity in actionsEntity)
            {
                 actionsDTO.Add(_actionMapper.ToDto(actionEntity));
            }
            return actionsDTO;
        }

        public NavObjectDTO GetTreeOfObjects()
        {
            throw new NotImplementedException();
        }
    }
}
