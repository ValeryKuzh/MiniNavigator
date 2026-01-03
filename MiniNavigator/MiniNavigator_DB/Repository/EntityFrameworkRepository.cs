using MiniNavigator_DB.Context;
using MiniNavigator_DB.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    /// <summary>
    /// Репозиторий для работы с данными в БД через EF
    /// </summary>
    /// <typeparam name="Entity">Сущность</typeparam>
    public class EntityFrameworkRepository<Entity> : IRepository<Entity> where Entity : class 
    {
        private readonly MiniNavigatorDbContext _context;
        private readonly DbSet<Entity> _dbSet;

        public EntityFrameworkRepository(MiniNavigatorDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Entity>();
        }

        /// <summary>
        /// Добавляет сущность в БД асинхронно
        /// </summary>
        /// <param name="entity">Сущность</param>
        public async Task AddAsync(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity не может быть null");
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет сущность из БД асинхронно
        /// </summary>
        /// <param name="entity">Сущность</param>
        public async Task DeleteAsync(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity не может быть null");
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет сущность по ID из БД асинхронно
        /// </summary>
        /// <param name="ID">ID сущности</param>
        public async Task DeleteByIDAsync(Guid ID)
        {
            if (ID == Guid.Empty) throw new ArgumentNullException("ID не может быть пустым");
            var entity = await _dbSet.FindAsync(ID);
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получает все сущности
        /// </summary>
        /// <returns>Перечисление сущностей</returns>
        public async Task<IEnumerable<Entity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Получает сущность по ID
        /// </summary>
        /// <param name="ID">ID сущности</param>
        /// <returns>Сущность</returns>
        public async Task<Entity> GetByIdAsync(Guid ID)
        {
            if (ID == Guid.Empty) throw new ArgumentNullException("ID не может быть пустым");
            return await _dbSet.FindAsync(ID);
        }

        /// <summary>
        /// Возвращает IQueryable-коллекцию для самостоятельной работы с коллекцией через LINQ
        /// </summary>
        /// <returns>IQueryable-коллекция сущностей</returns>
        public IQueryable<Entity> Query()
        {
            return _dbSet.AsQueryable();
        }

        /// <summary>
        /// Обновляет сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        public async Task UpdateAsync(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity не может быть null");
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Проверяет существует ли сущность по условию
        /// </summary>
        /// <param name="predicate">Условие проверки</param>
        /// <returns>Есть ли сущность в БД</returns>
        public async Task<bool> ExistsAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
