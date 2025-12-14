using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    public class ObjectTypeService : IObjectTypeService
    {
        private readonly IObjectTypeRepository _objectTypeRepository;

        private readonly IMapper<ObjectTypeDTO, ObjectType> _objectTypeMapper;
        
        public ObjectTypeService(
            IObjectTypeRepository objectTypeRepository, 
            
            IMapper<ObjectTypeDTO, ObjectType> objectTypeMapper
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
    }
}
