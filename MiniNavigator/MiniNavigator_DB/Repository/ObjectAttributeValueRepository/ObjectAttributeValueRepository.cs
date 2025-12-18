using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository.ObjectAttributeValueRepository
{
    public class ObjectAttributeValueRepository : EntityFrameworkRepository<ObjectAttributeValue>, IObjectAttributeValueRepository
    {
        private readonly MiniNavigatorDbContext _context;
        public ObjectAttributeValueRepository(MiniNavigatorDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ObjectAttributeValue> GetByIdAsync(Guid objID, Guid attrID)
        {
            return await _context.Set<ObjectAttributeValue>()
                .FindAsync(objID, attrID);
        }
    }
}
