using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    public class ObjectTypeRepository : EntityFrameworkRepository<ObjectType>, IObjectTypeRepository
    {
        private readonly MiniNavigatorDbContext _context;
        public ObjectTypeRepository(MiniNavigatorDbContext context) : base(context)
        {
            _context = context;
        }

        public async new Task<IEnumerable<ObjectType>> GetAllAsync()
        {
            return await _context.ObjectTypes
                                 .Include(ot => ot.Base).ToListAsync();
        }

        public async Task<ObjectType> GetTypeWithAttributesAsync(Guid id)
        {
            return await _context.ObjectTypes
                .Include(ot => ot.Base)
                .Include(ot => ot.Attributes)
                .Include(ot => ot.Attributes.Select(ota => ota.Attribute))
                .FirstOrDefaultAsync(ot => ot.Base_ID == id);
        }

        public async Task<ObjectType> GetTypeWithActionsAsync(Guid id)
        {
            return await _context.ObjectTypes
                .Include(ot => ot.Actions)
                .FirstOrDefaultAsync(ot => ot.Base_ID == id);
        }
    }
}
