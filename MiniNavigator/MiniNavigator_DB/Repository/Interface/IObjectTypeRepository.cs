
using MiniNavigator_DB.Model;
using System;
using System.Threading.Tasks;

namespace MiniNavigator_Services
{
    public interface IObjectTypeRepository
    {
        Task<ObjectType> GetByIdAsync(Guid ID);
    }
}
