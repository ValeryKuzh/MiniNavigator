using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using System;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository 
{
    /// <summary>
    /// Репозиторий, расширяющий основной, для ObjectAttributeValue
    /// </summary>
    public class ObjectAttributeValueRepository : EntityFrameworkRepository<ObjectAttributeValue>, IObjectAttributeValueRepository
    {
        private readonly MiniNavigatorDbContext _context;
        public ObjectAttributeValueRepository(MiniNavigatorDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает значение атрибута по составному ключу асинхронно
        /// </summary>
        /// <param name="objID">ID объекта</param>
        /// <param name="attrID">ID атрибута</param>
        /// <returns>Значение атрибута</returns>
        public async Task<ObjectAttributeValue> GetByIdAsync(Guid objID, Guid attrID)
        {
            if (objID == Guid.Empty) throw new ArgumentNullException("objID не может быть пустым");
            if (attrID == Guid.Empty) throw new ArgumentNullException("attrID не может быть пустым");
            return await _context.Set<ObjectAttributeValue>()
                .FindAsync(objID, attrID);
        }
    }
}
