using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        private readonly IRepository<ObjectFile> _objectFileRepository;
        private readonly IRepository<ObjectRole> _objectRoleRepository;
        private readonly IRepository<ObjectUser> _objectUserRepository;

        public ObjectService(
            IObjectTypeService objectTypeService,

            IMapper<NavObjectDTO, BaseObject> objectMapper,
            IMapper<ObjectActionDTO, ObjectAction> actionMapper,
            IMapper<ObjectAttributeDTO, ObjectAttributeValue> attributeMapper,

            IRepository<BaseObject> objectRepository,
            IObjectAttributeRepository objectAttributeRepository,
            IRepository<ObjectTypeAttribute> objectTypeAttributeRepository,
            IObjectTypeRepository objectTypeRepository,
            IObjectAttributeValueRepository objectAttributeValueRepository, 
            IRepository<ObjectFile> objectFileRepository,
            IRepository<ObjectRole> objectRoleRepository,
            IRepository<ObjectUser> objectUserRepository)
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
            _objectFileRepository = objectFileRepository;
            _objectRoleRepository = objectRoleRepository;
            _objectUserRepository = objectUserRepository;
        }

        /// <summary>
        /// Получает список действий для объекта
        /// </summary>
        /// <param name="ID">ID объекта</param>
        /// <returns>Список действий с объектом</returns>
        public async Task<List<ObjectActionDTO>> GetActionsForTypeObject(Guid ID)
        {
            var actionsDTO = new List<ObjectActionDTO>();
            if (ID == Guid.Empty)
                return actionsDTO;

            var baseObject = await _objectRepository.GetByIdAsync(ID);

            var typeOfObject = await _objectTypeRepository.GetTypeWithActionsAsync(baseObject.ObjectTypeID);

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

            var types = await GetSortedTypesAsync();

            foreach (var type in types.Where(t => t.IsVisible))
            {
                var typeObj = await _objectRepository.GetByIdAsync(type.ObjectID);
                if (typeObj.ObjectType == null)
                {
                    root.Children.Add(new NavObjectDTO
                    {
                        ID = type.ObjectID,
                        Title = type.Title,
                        ParentID = root.ID,
                        Parent = root
                    });
                }
                else
                {
                    var dto = new NavObjectDTO()
                    {
                        ID = typeObj.ID,
                        ObjectTypeID = typeObj.ObjectTypeID,
                        Title = type.Title,
                        Parent = null,
                        ParentID = typeObj.ParentID
                    };
                    AddObjectToTree(root, dto);
                }
            }

            return root;
        }

        /// <summary>
        /// Сортирует список обектов чтобы родители шли раньше детей
        /// </summary>
        /// <returns>Список отсортированных объектов</returns>
        private async Task<List<ObjectTypeDTO>> GetSortedTypesAsync()
        {
            var types = (await _objectTypeService.GetAllTypesAsync())
                .Where(t => t.IsVisible)
                .ToList();

            var typeObjectIds = types.Select(t => t.ObjectID).ToHashSet();

            var baseObjects = (await _objectRepository.GetAllAsync())
                .Where(o => typeObjectIds.Contains(o.ID))
                .ToList();

            var objectById = baseObjects.ToDictionary(o => o.ID);

            var childrenLookup = baseObjects
                .Where(o => o.ParentID != null)
                .GroupBy(o => o.ParentID.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<ObjectTypeDTO>();
            var visited = new HashSet<Guid>();

            void Traverse(BaseObject obj)
            {
                if (!visited.Add(obj.ID))
                    return;

                var type = types.First(t => t.ObjectID == obj.ID);
                result.Add(type);

                if (childrenLookup.TryGetValue(obj.ID, out var children))
                {
                    foreach (var child in children)
                    {
                        Traverse(child);
                    }
                }
            }

            foreach (var root in baseObjects.Where(o => o.ParentID == null))
            {
                Traverse(root);
            }

            return result;
        }

        /// <summary>
        /// Рекурсивно добавляет объект навигации в дерево
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="obj">Объект для добавления</param>
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

                attributeNameInfo[obj.ID] = new ObjectAttributeDTO
                {
                    ID = obj.ID,
                    Name = "ID",
                    Value = obj.ID.ToString(),
                    IsReference = false,
                    ValueType = typeof(Guid),
                    IsRequired = true,
                    IsVisible = false,
                    Index = -1
                };

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
        public async Task CreateObjectAsync(Guid typeID, ObjectDTO dto)
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
        public async Task<ObjectInfoDTO> GetObjectInfoByIdAsync(Guid id)
        {
            var obj = await _objectRepository.GetByIdAsync(id);
            if (obj == null)
                return null;

            string title = null;

            if (obj.ObjectTypeID.HasValue)
            {
                var type = await _objectTypeRepository
                    .GetTypeWithAttributesAsync(obj.ObjectTypeID.Value);

                var titleAttr = type.Attributes
                    .FirstOrDefault(a => a.Attribute.Name == "Title");

                if (titleAttr != null)
                {
                    var titleValue = await _objectAttributeValueRepository
                        .GetByIdAsync(obj.ID, titleAttr.AttributeID);

                    title = titleValue?.Value;
                }
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                title = await BuildObjectTitleAsync(obj);
            }

            return new ObjectInfoDTO
            {
                ID = obj.ID,
                Title = title
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
        
        /// <summary>
        /// Создает Title объекта из required-параметров для отображения на UI
        /// </summary>
        /// <param name="obj">Объект системы</param>
        /// <returns>Title объекта</returns>
        private async Task<string> BuildObjectTitleAsync(BaseObject obj)
        {
            var type = await _objectTypeRepository
                .GetTypeWithAttributesAsync(obj.ObjectTypeID.Value);

            var titleAttrs = type.Attributes
                .Where(a => a.IsTitle && !a.Attribute.IsReference)
                .OrderBy(a => a.Order)
                .ToList();

            var values = _objectAttributeValueRepository
                .Query()
                .Where(v => v.ObjectID == obj.ID)
                .ToList();

            var parts = titleAttrs
                .Select(a => values.FirstOrDefault(v => v.AttributeID == a.AttributeID)?.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v));

            return parts.Any()
                ? string.Join(" ", parts)
                : "(без названия)";
        }
        
        /// <summary>
        /// Обновляет данные объекта системы
        /// </summary>
        /// <param name="ID">ID объекта</param>
        /// <param name="dto">DTO объекта</param>
        /// <exception cref="ArgumentException">Объект не найден</exception>
        public async Task UpdateObjectAsync(Guid ID, ObjectDTO dto)
        {
            var obj = await _objectRepository.GetByIdAsync(ID);
            if (obj == null)
                throw new ArgumentException("Объект не найден");

            var type = await _objectTypeRepository
                .GetTypeWithAttributesAsync(obj.ObjectTypeID.Value);

            var attributes = type.Attributes;

            var existingValues = _objectAttributeValueRepository
                .Query()
                .Where(v => v.ObjectID == ID)
                .ToList();
                
            foreach (var attribute in attributes)
            {
                if (!dto.Attributes.TryGetValue(attribute.AttributeID, out var newValue))
                    continue;

                var existingValue = existingValues
                    .FirstOrDefault(v => v.AttributeID == attribute.AttributeID);

                if (existingValue != null)
                {
                    existingValue.Value = newValue.Value;
                    await _objectAttributeValueRepository.UpdateAsync(existingValue);
                }
                else
                {
                    var attributeValue = new ObjectAttributeValue
                    {
                        ObjectID = obj.ID,
                        Object = obj,
                        AttributeID = attribute.AttributeID,
                        Attribute = attribute.Attribute,
                        Value = newValue.Value
                    };

                    await _objectAttributeValueRepository.AddAsync(attributeValue);
                }
            }
        }

        /// <summary>
        /// Удаляет объект из системы
        /// </summary>
        /// <param name="objectId">ID объекта</param>
        /// <exception cref="InvalidOperationException">Если объект не найден или на объект есть действующие ссылки</exception>
        public async Task DeleteObjectAsync(Guid objectId)
        {
            if (objectId == Guid.Empty) return;
            var obj = await _objectRepository.GetByIdAsync(objectId);
            if (obj == null)
                throw new InvalidOperationException("Объект не найден");

            bool isReferenced = _objectAttributeValueRepository
                .Query()
                .Any(v =>
                    v.Attribute.IsReference &&
                    v.Value == objectId.ToString()
                );

            if (isReferenced)
                throw new InvalidOperationException(
                    "Невозможно удалить объект: на него существуют ссылки"
                );

            var values = _objectAttributeValueRepository
                .Query()
                .Where(v => v.ObjectID == objectId)
                .ToList();

            foreach (var value in values)
                await _objectAttributeValueRepository.DeleteAsync(value);

            await DeleteTypedEntitiesAsync(objectId);
            await _objectRepository.DeleteAsync(obj);
        }

        /// <summary>
        /// Удаляет объекты типов связанные с объектом
        /// </summary>
        /// <param name="objectId">ID удаляемого объекта</param>
        private async Task DeleteTypedEntitiesAsync(Guid objectId)
        {
            if (objectId == Guid.Empty) return;
            await DeleteIfExistsAsync(_objectUserRepository, x => x.Base_ID == objectId, x => x.ID);
            await DeleteIfExistsAsync(_objectRoleRepository, x => x.Base_ID == objectId, x => x.ID);
            await DeleteIfExistsAsync(_objectFileRepository, x => x.Base_ID == objectId, x => x.ID);
        }

        /// <summary>
        /// Проверяет есть ли такой объект типа и удаляет его
        /// </summary>
        /// <param name="repo">Репозиторий для удаления</param>
        /// <param name="predicate">Условие для удаления</param>
        /// <param name="idSelector">Выбор ID для объекта типа</param>
        /// <returns></returns>
        private async Task DeleteIfExistsAsync<TEntity>(
            IRepository<TEntity> repo,
            Func<TEntity, bool> predicate,
            Func<TEntity, Guid> idSelector) where TEntity : class
        {
            var entity = repo.Query().FirstOrDefault(predicate);
            if (entity != null)
                await repo.DeleteByIDAsync(idSelector(entity));
        }
    }
}