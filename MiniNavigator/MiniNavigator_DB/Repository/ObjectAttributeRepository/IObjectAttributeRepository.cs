using MiniNavigator_DB.Model;
using System;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.Interface
{
    /// <summary>
    /// Интерфейс, расширяемый для ObjectAttribute
    /// </summary>
    public interface IObjectAttributeRepository : IRepository<ObjectAttribute>
    {
        Task<ObjectAttribute> GetAttributeWithObjectTypesAsync(Guid attributeId);
    }
}
