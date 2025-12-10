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

        public async Task<List<NavObjectDTO>> GetTreeOfObjectsAsync()
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

            var dbObjects = await _objectRepository.GetAllAsync();

            // 2. Маппим к DTO
            var flatList = dbObjects.Select(o => new NavObjectDTO
            {
                ID = o.ID,
                ObjectTypeID = o.ObjectTypeID,
                ParentID = o.ParentID
            }).ToList();

            // 3. Создаём lookup по ID
            var lookup = flatList.ToDictionary(x => x.ID);

            // 4. Список корневых узлов
            var roots = new List<NavObjectDTO>();

            // 5. Собираем дерево
            foreach (var dto in flatList)
            {
                if (dto.ParentID != null && lookup.ContainsKey(dto.ParentID.Value))
                {
                    // Родитель найден — добавляем в Children
                  //  lookup[dto.ParentID.Value].Children.Add(dto);
                }
                else
                {
                    // Это корневой объект
                    roots.Add(dto);
                }
            }

            return roots;
        }
    }
}
