using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    public interface IObjectAttributeRepository
    {
        Task<ObjectAttribute> GetAttributeWithObjectTypesAsync(Guid attributeId);
        IQueryable<ObjectAttribute> Query();
    }
}
