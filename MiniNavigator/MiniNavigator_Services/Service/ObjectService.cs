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
        private readonly IRepository<ObjectType> _objectTypeRepository;

        public ObjectService(
            IObjectTypeService objectTypeService,

            IMapper<NavObjectDTO, BaseObject> objectMapper,
            IMapper<ObjectActionDTO, ObjectAction> actionMapper,
            
            IRepository<ObjectType> objectTypeRepository,
            IRepository<BaseObject> objectRepository)
        {
            _objectTypeService = objectTypeService;
            
            _objectMapper = objectMapper;
            _actionMapper = actionMapper;
            
            _objectTypeRepository = objectTypeRepository;
            _objectRepository = objectRepository;
        }

        /// <summary>
        /// Получает список действий для объекта
        /// </summary>
        /// <param name="ID">ID объекта</param>
        /// <returns>Список действий с объектом</returns>
        public async Task<List<ObjectActionDTO>> GetActionsForObject(Guid ID)
        {
            var actionsDTO = new List<ObjectActionDTO>();
            if (ID == Guid.Empty)
                return actionsDTO;

            var baseObject = _objectRepository.Query().Where(bo => bo.ID == ID).FirstOrDefault();

            var typeOfObject = _objectTypeRepository.Query().Where(ot => ot.ID == baseObject.ObjectTypeID).FirstOrDefault();

            if (!(typeOfObject is null))
            {
                foreach (var actionEntity in typeOfObject?.Actions)
                {
                    actionsDTO.Add(_actionMapper.ToDTO(actionEntity));
                }
            }
            return actionsDTO;
        }

        /// <summary>
        /// Строит дерево объектов из GUID 
        /// </summary>
        /// <returns>Дерево объектов</returns>
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
                if(type.IsVisible)
                    root.Children.Add(objType);
            }

            var baseObjects = await _objectRepository.GetAllAsync();

            foreach (var baseObject in baseObjects)
            {
                if (baseObjects.Where(bo => bo.ID == baseObject.ParentID) == null) // если тип объекта - пропускаем
                {
                    continue;
                }
                else
                {
                    var navObjectDTO = _objectMapper.ToDTO(baseObject);
                    AddObjectToTree(root, navObjectDTO);
                }
            }

            return root;
        }

        private void AddObjectToTree(NavObjectDTO root, NavObjectDTO obj) 
        {
            if (obj.ParentID == root.ID)
            {
                obj.Parent = root;
                root.Children.Add(obj);
            }
            else
            {
                foreach (var child in root.Children)
                {
                    AddObjectToTree(child, obj);
                }
            }
        }
    }
}