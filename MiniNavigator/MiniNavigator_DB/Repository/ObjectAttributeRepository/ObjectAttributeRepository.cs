using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    /// <summary>
    /// Репозиторий, расширяющий основной, для ObjectAttribute
    /// </summary>
    public class ObjectAttributeRepository : EntityFrameworkRepository<ObjectAttribute>, IObjectAttributeRepository
    {
        private readonly MiniNavigatorDbContext _context;
        public ObjectAttributeRepository(MiniNavigatorDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает атрибут со связью с типами асихнхронно
        /// </summary>
        /// <param name="attributeID">ID атрибута</param>
        /// <returns>Атрибут</returns>
        public async Task<ObjectAttribute> GetAttributeWithObjectTypesAsync(Guid attributeID)
        {
            if (attributeID == Guid.Empty) throw new ArgumentNullException("attributeID не может быть пустым");
            return await _context.ObjectAttributes
                .Include(a => a.ObjectTypes)
                .Include(a => a.ObjectTypes.Select(ota => ota.ObjectType))
                .Include(a => a.ObjectTypes.Select(ota => ota.ObjectType.Base))
                .Include(a => a.ReferenceObjectType)
                .FirstOrDefaultAsync(a => a.ID == attributeID);
        }
    }
}
