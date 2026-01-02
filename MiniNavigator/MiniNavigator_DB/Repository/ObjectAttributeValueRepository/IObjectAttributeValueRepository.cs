using MiniNavigator_DB.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.Interface
{
    public interface IObjectAttributeValueRepository : IRepository<ObjectAttributeValue>
    {
        Task<ObjectAttributeValue> GetByIdAsync(Guid objID, Guid attrID);
        Task AddAsync(ObjectAttributeValue objectAttributeValue);
        IQueryable<ObjectAttributeValue> Query();
    }
}
