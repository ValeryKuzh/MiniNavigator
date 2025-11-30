using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid ID);
        IEnumerable<T> GetAll();
        IEnumerable<T> FindAll(Func<T, bool> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        bool Exists(T entity);
        Task<T> GetByIdAsync(Guid ID);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAllAsync(Func<T, bool> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
