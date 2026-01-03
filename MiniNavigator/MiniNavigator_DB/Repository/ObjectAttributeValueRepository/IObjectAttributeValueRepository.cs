using MiniNavigator_DB.Model;
using System;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.Interface
{
    public interface IObjectAttributeValueRepository : IRepository<ObjectAttributeValue>
    {
        /// <summary>
        /// Интерфейс, расширяемый для ObjectAttributeValue
        /// </summary>
        Task<ObjectAttributeValue> GetByIdAsync(Guid objID, Guid attrID);
    }
}
