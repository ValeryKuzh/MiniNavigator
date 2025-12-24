using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    public class ObjectService : IObjectService
    {
        private readonly IObjectTypeService _objectTypeService;

        private readonly IMapper<NavObjectDTO, BaseObject> _objectMapper;
        private readonly IMapper<ObjectActionDTO, ObjectAction> _actionMapper;
        private readonly IMapper<ObjectAttributeDTO, ObjectAttributeValue> _attributeMapper;

        private readonly IRepository<BaseObject> _objectRepository;
        private readonly IObjectAttributeRepository _objectAttributeRepository;
        private readonly IRepository<ObjectTypeAttribute> _objectTypeAttributeRepository;
        private readonly IObjectTypeRepository _objectTypeRepository;
        private readonly IObjectAttributeValueRepository _objectAttributeValueRepository;

        public ObjectService(
            IObjectTypeService objectTypeService,

            IMapper<NavObjectDTO, BaseObject> objectMapper,
            IMapper<ObjectActionDTO, ObjectAction> actionMapper,
            IMapper<ObjectAttributeDTO, ObjectAttributeValue> attributeMapper,

            IRepository<BaseObject> objectRepository,
            IObjectAttributeRepository objectAttributeRepository,
            IRepository<ObjectTypeAttribute> objectTypeAttributeRepository,
            IObjectTypeRepository objectTypeRepository,
            IObjectAttributeValueRepository objectAttributeValueRepository)
        {
            _objectTypeService = objectTypeService;
            
            _objectMapper = objectMapper;
            _actionMapper = actionMapper;
            _attributeMapper = attributeMapper;

            _objectRepository = objectRepository;
            _objectAttributeRepository = objectAttributeRepository;
            _objectTypeAttributeRepository = objectTypeAttributeRepository;
            _objectTypeRepository = objectTypeRepository;
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
        /// Получает список объектов с атрибутами определенного типа
        /// </summary>
        /// <param name="ID">ID типа</param>
        /// <returns>Список объектов</returns>
        public async Task<List<Dictionary<Guid, ObjectAttributeDTO>>> GetTableData(Guid ID)
        {
            var result = new List<Dictionary<Guid, ObjectAttributeDTO>>();

            var objectsOfType = _objectRepository.Query().Where(bo => bo.ObjectTypeID == ID).ToList();

            var objectTypeAttributes = await GetAttributesForTypeAsync(ID);

            foreach (var obj in objectsOfType)
            {
                var attributeNameInfo = new Dictionary<Guid, ObjectAttributeDTO>();

                foreach (var attr in objectTypeAttributes)
                {
                    var value = _objectAttributeValueRepository
                        .Query()
                        .Where(av => av.ObjectID == obj.ID && av.AttributeID == attr.Attribute.ID)
                        .FirstOrDefault();
                    if (value != null)
                    {
                        var valueDTO = _attributeMapper.ToDTO(value);
                        valueDTO.IsRequired = attr.IsRequired;
                        valueDTO.IsVisible = attr.IsVisible;
                        valueDTO.Index = attr.Order;
                        attributeNameInfo[attr.Attribute.ID] = valueDTO;
                    }
                    else
                        attributeNameInfo[attr.Attribute.ID] = null;
                }
            
                result.Add(attributeNameInfo);
            }

            return result;
        }

        /// <summary>
        /// Получение списка атрибутов конктретного объекта 
        /// </summary>
        /// <param name="ID">ID объекта</param>
        /// <returns>Список атрибутов</returns>
        private async Task<List<ObjectTypeAttribute>> GetAttributesForTypeAsync(Guid ID)
        {
            var attributes = new List<ObjectTypeAttribute>();

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

        /// <summary>
        /// Создает новый объект в системе со всеми атрибутами
        /// </summary>
        /// <param name="typeID">ID типа</param>
        /// <param name="dto">DTO для создания объекта</param>
        /// <exception cref="ArgumentException">Если не передан обязательный атрибут</exception>
        public async Task CreateObjectAsync(Guid typeID, CreateObjectDTO dto)
        {
            var typeObject = await _objectRepository.GetByIdAsync(typeID);
            var attributes = (await _objectTypeRepository.GetTypeWithAttributesAsync(typeID)).Attributes;

            foreach (var attribute in attributes)
            {
                if (attribute.IsRequired &&
                    !dto.Attributes.ContainsKey(attribute.AttributeID))
                {
                    throw new ArgumentException(
                        $"Атрибут {attribute.Attribute.Name} обязательный!"
                    );
                }
            }

            var newObject = new BaseObject
            {
                ID = dto.ID,
                ObjectType = typeObject,
                Parent = typeObject,
                ParentID = typeObject.ID,
            };

            await _objectRepository.AddAsync(newObject);

            foreach (var attribute in attributes)
            {
                if (!dto.Attributes.TryGetValue(attribute.AttributeID, out var value))
                    continue;

                var attributeValue = new ObjectAttributeValue
                {
                    ObjectID = newObject.ID,
                    Object = newObject,
                    AttributeID = attribute.AttributeID,
                    Attribute = attribute.Attribute,
                    Value = value.Value
                };

                await _objectAttributeValueRepository.AddAsync(attributeValue);
            }
        }

        /// <summary>
        /// Получает Title объекта для отображения списка
        /// </summary>
        /// <param name="ID">ID объекта</param>
        /// <returns>Объект с информацией об объекте</returns>
        public async Task<ObjectInfoDTO> GetObjectInfoByIdAsync(Guid ID)
        {
            var obj = await _objectRepository.GetByIdAsync(ID);
            
            var attributes = (await _objectTypeRepository.GetTypeWithAttributesAsync((Guid)obj.ObjectTypeID)).Attributes;
            ObjectAttributeValue titleAttribute = null;
            foreach(var attr in attributes)
            {
                if(attr.Attribute.Name == "Title")
                    titleAttribute = await _objectAttributeValueRepository.GetByIdAsync(obj.ID, attr.AttributeID);
            }
            return new ObjectInfoDTO()
            {
                ID = obj.ID,
                Title = titleAttribute.Value
            };
        }

        /// <summary>
        /// Получает список информации об объектах  
        /// </summary>
        /// <param name="attributeId">ID атрибута</param>
        /// <returns>Список объектов</returns>
        public async Task<List<ObjectInfoDTO>> GetReferenceObjectInfosByAttribute(Guid attributeID)
        {
            var attribute = await _objectAttributeRepository
                .GetAttributeWithObjectTypesAsync(attributeID);

            if (attribute?.ReferenceObjectType == null)
                return new List<ObjectInfoDTO>();

            // получаем все объекты нужного типа
            var objects = _objectRepository.Query()
                .Where(o => o.ObjectTypeID == attribute.ReferenceObjectType.Base_ID)
                .ToList();

            var result = new List<ObjectInfoDTO>();

            foreach (var obj in objects)
            {
                var title = await BuildObjectTitleAsync(obj);

                result.Add(new ObjectInfoDTO
                {
                    ID = obj.ID,
                    Title = title
                });
            }

            return result;
        }

        private async Task<string> BuildObjectTitleAsync(BaseObject obj)
        {
            var type = await _objectTypeRepository
                .GetTypeWithAttributesAsync(obj.ObjectTypeID.Value);

            var requiredAttrs = type.Attributes
                .Where(a => a.IsRequired)
                .OrderBy(a => a.Order)
                .ToList();

            var values = _objectAttributeValueRepository
                .Query()
                .Where(v => v.ObjectID == obj.ID)
                .ToList();

            var parts = requiredAttrs
                .Select(a => values.FirstOrDefault(v => v.AttributeID == a.AttributeID)?.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v));

            return parts.Any()
                ? string.Join(" ", parts)
                : "(без названия)";
        }
    }
}