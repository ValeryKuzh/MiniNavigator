using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<T> GetByIdAsync(Guid ID);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
