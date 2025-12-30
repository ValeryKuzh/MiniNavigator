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
    public class EntityFrameworkRepository<Entity> : IRepository<Entity> where Entity : class 
    {
        private readonly MiniNavigatorDbContext _context;
        private readonly DbSet<Entity> _dbSet;

        public EntityFrameworkRepository(MiniNavigatorDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Entity>();
        }

        public async Task AddAsync(Entity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Entity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIDAsync(Guid ID)
        {
            var entity = await _dbSet.SingleOrDefaultAsync();
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Entity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<Entity> Query()
        {
            return _dbSet.AsQueryable();
        }

        public async Task UpdateAsync(Entity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
