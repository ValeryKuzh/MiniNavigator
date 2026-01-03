using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.Interface
{
    /// <summary>
    /// Общий интерфейс репозитория для базовых операций с сущностями
    /// </summary>
    /// <typeparam name="Entity">Сущность</typeparam>
    public interface IRepository<Entity> where Entity : class
    {
        Task<Entity> GetByIdAsync(Guid ID);
        Task<IEnumerable<Entity>> GetAllAsync();
        Task AddAsync(Entity entity);
        Task UpdateAsync(Entity entity);
        Task DeleteAsync(Entity entity);
        Task DeleteByIDAsync(Guid ID);
        IQueryable<Entity> Query();
        Task<bool> ExistsAsync(Expression<Func<Entity, bool>> predicate);
    }
}
