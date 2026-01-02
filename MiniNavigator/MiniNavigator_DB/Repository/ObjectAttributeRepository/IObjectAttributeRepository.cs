using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.Interface
{
    public interface IObjectAttributeRepository : IRepository<ObjectAttribute>
    {
        Task<ObjectAttribute> GetAttributeWithObjectTypesAsync(Guid attributeId);
    }
}
