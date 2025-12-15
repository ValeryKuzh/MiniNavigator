
using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.Interface
{
    public interface IObjectTypeRepository
    {
        Task<ObjectType> GetByIdAsync(Guid ID);
        Task<IEnumerable<ObjectType>> GetAllAsync();
        IQueryable<ObjectType> Query();
        Task<ObjectType> GetTypeWithAttributesAsync(Guid ID);
        Task<ObjectType> GetTypeWithActionsAsync(Guid ID);
    }
}
