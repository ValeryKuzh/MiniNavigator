using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    public class ObjectTypeService : IObjectTypeService
    {
        private readonly IObjectTypeRepository _objectTypeRepository;

        private readonly IMapper<ObjectTypeDTO, ObjectType> _objectTypeMapper;

        public ObjectTypeService(
            IObjectTypeRepository objectTypeRepository,

            IMapper<ObjectTypeDTO, ObjectType> objectTypeMapper, 
            IMapper<ObjectActionDTO, ObjectAction> actionMapper
            )
        {
            _objectTypeRepository = objectTypeRepository;

            _objectTypeMapper = objectTypeMapper;
        }

        public async Task<List<ObjectTypeDTO>> GetAllTypesAsync()
        {
            List<ObjectTypeDTO> types = new List<ObjectTypeDTO>();
            foreach(var typeEntity in await _objectTypeRepository.GetAllAsync())
            {
                types.Add(_objectTypeMapper.ToDTO(typeEntity));
            }
            return types;
        }

        public async Task<ObjectTypeDTO> GetTypeByIDAsync(Guid typeID)
        {
            return _objectTypeMapper.ToDTO(await _objectTypeRepository.GetByIdAsync(typeID));
        }

        public async Task<List<ObjectAttributeDTO>> GetAttributesForTypeAsync(Guid objectTypeId)
        {
            var typeAttributes = (await _objectTypeRepository.GetTypeWithAttributesAsync(objectTypeId)).Attributes.ToList();

            return typeAttributes.Select(ota => new ObjectAttributeDTO
            {
                ID = ota.Attribute.ID,
                Name = ota.Attribute.Name,
                ValueType = ota.Attribute.ValType,

                IsReference = ota.Attribute.IsReference,
                IsRequired = ota.IsRequired,
                IsVisible = ota.IsVisible,

                Index = ota.Order
            }).ToList();
        }
    }
}
