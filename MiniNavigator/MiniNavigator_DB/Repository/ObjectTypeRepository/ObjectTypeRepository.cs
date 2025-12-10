using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<ObjectType>> GetAllAsync()
        {
            return await _context.ObjectTypes
                                 .Include("Base")
                                 .ToListAsync();
        }
    }
}
