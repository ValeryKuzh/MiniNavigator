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
    /// <summary>
    /// Сервис для работы с типами объектов системы
    /// </summary>
    public class ObjectTypeService : IObjectTypeService
    {
        private readonly IObjectTypeRepository _objectTypeRepository;
        private readonly IRepository<BaseObject> _objectRepository;

        private readonly IMapper<ObjectTypeDTO, ObjectType> _objectTypeMapper;

        public ObjectTypeService(
            IObjectTypeRepository objectTypeRepository,
            IRepository<BaseObject> objectRepository,

            IMapper<ObjectTypeDTO, ObjectType> objectTypeMapper, 
            IMapper<ObjectActionDTO, ObjectAction> actionMapper
            )
        {
            _objectTypeRepository = objectTypeRepository;
            _objectRepository = objectRepository;

            _objectTypeMapper = objectTypeMapper;
        }

        /// <summary>
        /// Получение всех типов системы
        /// </summary>
        /// <returns>Список типов системы</returns>
        public async Task<List<ObjectTypeDTO>> GetAllTypesAsync()
        {
            List<ObjectTypeDTO> types = new List<ObjectTypeDTO>();
            foreach(var typeEntity in await _objectTypeRepository.GetAllAsync())
            {
                types.Add(_objectTypeMapper.ToDTO(typeEntity));
            }
            return types;
        }

        /// <summary>
        /// Получает тип по ID объекта типа
        /// </summary>
        /// <param name="objectTypeID">ID объекта типа</param>
        /// <returns>Тип</returns>
        public async Task<ObjectTypeDTO> GetTypeByTypeObjectIDAsync(Guid objectTypeID)
        {
            if (objectTypeID == Guid.Empty) throw new ArgumentNullException(nameof(objectTypeID));
            var objectOfType = await _objectRepository.GetByIdAsync(objectTypeID);
            var type = _objectTypeRepository.Query().Where(x => x.Base_ID == objectOfType.ID).FirstOrDefault();
            return _objectTypeMapper.ToDTO(type);
        }

        /// <summary>
        /// ПОлучает список атрибутов, валидных для типа
        /// </summary>
        /// <param name="objectTypeID">ID объекта типа</param>
        /// <returns>Список атрибутов типа</returns>
        public async Task<List<ObjectAttributeDTO>> GetAttributesForTypeAsync(Guid objectTypeID)
        {
            if (objectTypeID == Guid.Empty) throw new ArgumentNullException(nameof(objectTypeID));

            var typeAttributes = (await _objectTypeRepository.GetTypeWithAttributesAsync(objectTypeID)).Attributes.ToList();

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

        /// <summary>
        /// Проверяет является ли тип типом "Файл"
        /// </summary>
        /// <param name="objectTypeID">ID объекта типа</param>
        /// <returns>Тип является типом "Файл"</returns>
        public async Task<bool> IsFileTypeAsync(Guid objectTypeID)
        {
            if (objectTypeID == Guid.Empty) throw new ArgumentNullException(nameof(objectTypeID));

            var type = await _objectTypeRepository.GetByIdAsync(objectTypeID);
            if (type == null)
                return false;

            if (type.Name == "Файл")
                return true;

            if (type.Base != null && type.Base.ObjectTypeID.HasValue)
            {
                var parentObj = await _objectRepository.GetByIdAsync(type.Base_ID);
                var parentType = _objectTypeRepository.Query().Where(x => x.Base_ID == parentObj.ObjectTypeID.Value).FirstOrDefault();

                return parentType?.Name == "Файл";
            }

            return false;
        }
    }
}
