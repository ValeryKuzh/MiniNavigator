using MiniNavigator_Services.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service.Interface
{
    public interface IObjectTypeService
    {
        Task<List<ObjectTypeDTO>> GetAllTypesAsync();
        Task<ObjectTypeDTO> GetTypeByTypeObjectIDAsync(Guid typeID);
        Task<List<ObjectAttributeDTO>> GetAttributesForTypeAsync(Guid objectTypeId);
        Task<bool> IsFileTypeAsync(Guid objectTypeId);
    }
}