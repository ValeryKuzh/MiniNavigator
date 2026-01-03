using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    /// <summary>
    /// Сервис для работы с атрибутами
    /// </summary>
    public class AttributeService : IAttributeService
    {
        private readonly IRepository<BaseObject> _objectRepository;
        private readonly IRepository<ObjectTypeAttribute> _objectTypeAttributeRepository;
        private readonly IObjectTypeRepository _objectTypeRepository;
        private readonly IObjectAttributeRepository _objectAttributeRepository;

        public AttributeService
            (
            IRepository<BaseObject> objectRepository,
            IRepository<ObjectTypeAttribute> objectTypeAttributeRepository,
            IObjectTypeRepository objectTypeRepository,
            IObjectAttributeRepository objectAttributeRepository
            )
        {
            _objectRepository = objectRepository;
            _objectTypeAttributeRepository = objectTypeAttributeRepository;
            _objectTypeRepository = objectTypeRepository;
            _objectAttributeRepository = objectAttributeRepository;
        }

        /// <summary>
        /// Добавляет новый атрибут в систему
        /// </summary>
        /// <param name="objectAttributeDTO">DTO с информацией об атрибуте</param>
        /// <param name="typeID">ID типа, к которому привязан атрибут</param>
        public async Task CreateAttributeAsync(ObjectAttributeDTO objectAttributeDTO, Guid typeID)
        {
            if(objectAttributeDTO == null) throw new ArgumentNullException(nameof(objectAttributeDTO));
            if(typeID == Guid.Empty) throw new ArgumentNullException(nameof(typeID));

            var attributeType = _objectTypeRepository.Query().Where(x => x.Name == "Атрибут").First();
            var attributeTypeObject = _objectRepository.Query().Where(bo => bo.ObjectTypeID == attributeType.Base_ID).First();
            var attrObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = attributeTypeObject.ID,
                ParentID = attributeTypeObject.ID
            };
            await _objectRepository.AddAsync(attrObject);

            var attribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base_ID = attrObject.ID,
                Base = attrObject,
                Name = objectAttributeDTO.Name,
                IsReference = objectAttributeDTO.IsReference,
                ValueType = objectAttributeDTO.ValueType.FullName,
                ReferenceObjectTypeID = objectAttributeDTO.ReferenceObjectTypeID
            };
            await _objectAttributeRepository.AddAsync(attribute);

            var objectTypeAttribute = new ObjectTypeAttribute
            {
                ObjectTypeID = typeID,
                AttributeID = attribute.ID,
                Attribute = attribute,
                IsRequired = objectAttributeDTO.IsRequired,
                IsTitle = objectAttributeDTO.IsTitle,
                IsVisible = objectAttributeDTO.IsVisible,
                Order = objectAttributeDTO.Index
            };
            await _objectTypeAttributeRepository.AddAsync(objectTypeAttribute);
        }
    }
}
