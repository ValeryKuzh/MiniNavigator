using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service.AttributeService
{
    public class AttributeService
    {
        private readonly IRepository<BaseObject> _objectRepository;
        private readonly IRepository<ObjectTypeAttribute> _objectTypeAttributeRepository;
        private readonly IObjectTypeRepository _objectTypeRepository;
        private readonly IObjectAttributeRepository _objectAttributeRepository;

        private readonly IMapper<ObjectAttributeDTO, ObjectTypeAttribute> _attributeTypeMapper = new AttributeTypeMapper();
        private readonly IMapper<ObjectAttributeDTO, ObjectAttributeValue> _attributeMapper = new AttributeMapper();
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
        /// 
        /// </summary>
        /// <param name="objectAttributeDTO">DTO с информацией об атрибуте</param>
        /// <param name="typeID">ID типа, к которому привязан атрибут</param>
        /// <returns></returns>
        public async Task CreateAttribute(ObjectAttributeDTO objectAttributeDTO, Guid typeID)
        {
            var attributeType = _objectTypeRepository.Query().Where(x => x.Name == "Атрибут").First();
            var attrObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = attributeType.ID,
                ParentID = attributeType.ID
            };
            await _objectRepository.AddAsync(attrObject);

            var attribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
            };
            //await _objectAttributeRepository.AddAsync();
        }
    }
}
