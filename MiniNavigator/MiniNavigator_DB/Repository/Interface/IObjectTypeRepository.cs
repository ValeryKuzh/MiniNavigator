
using MiniNavigator_DB.Model;
using System;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.Interface
{
    public interface IObjectTypeRepository
    {
        Task<ObjectType> GetByIdAsync(Guid ID);
    }
}
