using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniNavigator_Services.DTO;

namespace MiniNavigator_Services.Service.Interface
{
    public interface IObjectService
    {
        Task<List<ObjectActionDTO>> GetActionsForObject(Guid ID);
        Task<NavObjectDTO> GetTreeOfObjectsAsync();
        Task<List<Dictionary<Guid, ObjectAttributeDTO>>> GetTableData(Guid ID);
    }
}
