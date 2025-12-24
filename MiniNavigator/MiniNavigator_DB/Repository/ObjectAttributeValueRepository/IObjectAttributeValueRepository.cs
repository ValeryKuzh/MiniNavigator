using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    public interface IObjectAttributeValueRepository
    {
        Task<ObjectAttributeValue> GetByIdAsync(Guid objID, Guid attrID);
        Task AddAsync(ObjectAttributeValue objectAttributeValue);
        IQueryable<ObjectAttributeValue> Query();
    }
}
