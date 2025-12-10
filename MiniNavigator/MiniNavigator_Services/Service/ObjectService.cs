using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;

namespace MiniNavigator_Services.Service
{
    public class ObjectService : IObjectService
    {
        private readonly IObjectTypeService _objectTypeService;
        private readonly IMapper<NavObjectDTO, BaseObject> _objectMapper;
        private readonly IMapper<ObjectActionDTO, ObjectAction> _actionMapper;

        private readonly IRepository<BaseObject> _objectRepository;

        public ObjectService(IObjectTypeService objectTypeService, IMapper<NavObjectDTO, BaseObject> objectMapper, IMapper<ObjectActionDTO, ObjectAction> actionMapper, IRepository<BaseObject> objectRepository)
        {
            _objectTypeService = objectTypeService;
            _objectMapper = objectMapper;
            _actionMapper = actionMapper;
            _objectRepository = objectRepository;
        }

        public async Task<List<ObjectActionDTO>> GetActionsForObject(Guid ID)
        {
            var obj = await _objectRepository.GetByIdAsync(ID);
            //var actionsEntity = obj.ObjectType.Actions.ToList();
            var actionsDTO = new List<ObjectActionDTO>();
            //foreach(var actionEntity in actionsEntity)
            //{
            //     actionsDTO.Add(_actionMapper.ToDTO(actionEntity));
            //}
            return actionsDTO;
        }

        public async Task<NavObjectDTO> GetTreeOfObjectsAsync()
        {
            var root = new NavObjectDTO()
            {
                Title = "Система"
            };

            foreach (var type in await _objectTypeService.GetAllObjectTypesAsync())
            {
                var objType = new NavObjectDTO()
                {
                    ID = type.ObjectID,
                    ObjectTypeID = null,
                    Title = type.Title,
                    Parent = root,
                    ParentID = root.ID,
                };
                root.Children.Add(objType);
            }

            //var objectsWithoutTypes = _objectRepository.Query().Where(o => !root.Children.Select(obj => obj.ID).Contains(o.ID)).ToList();
            
            return root;
        }
    }
}
