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
        private readonly IMapper<ObjectAttributeDTO, ObjectAttributeValue> _attributeMapper;

        private readonly IRepository<BaseObject> _objectRepository;
        private readonly IObjectTypeRepository _objectTypeRepository;
        private readonly IRepository<ObjectAttributeValue> _objectAttributeValueRepository;

        public ObjectService(
            IObjectTypeService objectTypeService,

            IMapper<NavObjectDTO, BaseObject> objectMapper,
            IMapper<ObjectActionDTO, ObjectAction> actionMapper,
            IMapper<ObjectAttributeDTO, ObjectAttributeValue> attributeMapper,

            IObjectTypeRepository objectTypeRepository,
            IRepository<BaseObject> objectRepository, 
            IRepository<ObjectAttributeValue> objectAttributeValueRepository)
        {
            _objectTypeService = objectTypeService;
            
            _objectMapper = objectMapper;
            _actionMapper = actionMapper;
            _attributeMapper = attributeMapper;
            
            _objectTypeRepository = objectTypeRepository;
            _objectRepository = objectRepository;
            _objectAttributeValueRepository = objectAttributeValueRepository;
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

            var baseObject = await _objectRepository.GetByIdAsync(ID);

            var typeOfObject = await _objectTypeRepository.GetTypeWithActionsAsync(baseObject.ID);

            if (!(typeOfObject.Actions is null))
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
            
            foreach (var type in await _objectTypeService.GetAllTypesAsync())
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<List<Dictionary<Guid, ObjectAttributeDTO>>> GetTableData(Guid ID)
        {
            var result = new List<Dictionary<Guid, ObjectAttributeDTO>>();

            var objectsOfType = _objectRepository.Query().Where(bo => bo.ObjectTypeID == ID).ToList();

            var attributesOfType = await GetAttributesForTypeAsync(ID);

            foreach (var obj in objectsOfType)
            {
                var attributeNameInfo = new Dictionary<Guid, ObjectAttributeDTO>();

                foreach (var attr in attributesOfType)
                {
                    var value = _objectAttributeValueRepository
                        .Query()
                        .Where(av => av.ObjectID == obj.ID && av.AttributeID == attr.ID)
                        .FirstOrDefault();
                    if (value != null)
                        attributeNameInfo[attr.ID] = _attributeMapper.ToDTO(value);
                    else
                        attributeNameInfo[attr.ID] = null;
                }

                result.Add(attributeNameInfo);
            }

            return result;
        }

        private async Task<List<ObjectAttribute>> GetAttributesForTypeAsync(Guid ID)
        {
            var attributes = new List<ObjectAttribute>();

            var typeObject = await _objectRepository.GetByIdAsync(ID);
            if (typeObject != null)
            {
                var type = await _objectTypeRepository.GetTypeWithAttributesAsync(typeObject.ID);
                if (type != null)
                {
                    foreach (var attr in type?.Attributes)
                    {
                        attributes.Add(attr);
                    }
                }
            }
            return attributes;
        }

        public async Task CreateObjectAsync(Guid typeID, CreateObjectDTO dto)
        {
            var typeObject = await _objectRepository.GetByIdAsync(typeID);
            var newObject = new BaseObject()
            {
                ID = dto.ID,
                ObjectType = typeObject,
                Parent = typeObject,
                ParentID = typeObject.ID,
            };

            await _objectRepository.AddAsync(newObject);

            var attributes = (await _objectTypeRepository.GetTypeWithAttributesAsync(typeID)).Attributes;

            foreach (var attribute in attributes)
            {
                if (!dto.Attributes.TryGetValue(attribute.ID, out var value))
                    continue; // атрибут не передан — пропускаем

                var attributeValue = new ObjectAttributeValue
                {
                    ObjectID = newObject.ID,
                    Object = newObject,
                    AttributeID = attribute.ID,
                    Attribute = attribute,
                    Value = value.Value
                };

                await _objectAttributeValueRepository.AddAsync(attributeValue);
            }
        }
    }
}