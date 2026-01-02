using MiniNavigator_DB.Model;
using MiniNavigator_Services.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service.Interface
{
    public interface IObjectService
    {
        Task<List<ObjectActionDTO>> GetActionsForTypeObject(Guid ID);
        Task<NavObjectDTO> GetTreeOfObjectsAsync();
        Task<List<Dictionary<Guid, ObjectAttributeDTO>>> GetTableData(Guid ID);
        Task CreateObjectAsync(Guid typeID, ObjectDTO dto);
        Task UpdateObjectAsync(Guid ID, ObjectDTO dto);
        Task DeleteObjectAsync(Guid objectId);
        Task<ObjectInfoDTO> GetObjectInfoByIdAsync(Guid typeID);
        Task<List<ObjectInfoDTO>> GetReferenceObjectInfosByAttribute(Guid attributeID);
    }
}
