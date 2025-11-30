using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid ID);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> Query();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
