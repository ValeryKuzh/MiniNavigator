using MiniNavigator_UI.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniNavigator_UI
{
    public interface IObjectService
    {
        Task<List<ObjectActionDTO>> GetActionsForObject(Guid ID);
        NavObjectDTO GetTreeOfObjects();
    }
}
