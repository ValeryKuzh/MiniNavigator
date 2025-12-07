using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository;
using MiniNavigator_Service.Service.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper;

namespace MiniNavigator_Service.Service
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
            var obj = await _objectRepository.GetByIdAsync(ID);
            var actionsEntity = obj.ObjectType.Actions.ToList();
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
