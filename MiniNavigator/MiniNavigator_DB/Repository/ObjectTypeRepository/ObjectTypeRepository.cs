using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Repository
{
    /// <summary>
    /// Репозиторий, расширяющий основной, для ObjectType
    /// </summary>
    public class ObjectTypeRepository : EntityFrameworkRepository<ObjectType>, IObjectTypeRepository
    {
        private readonly MiniNavigatorDbContext _context;
        public ObjectTypeRepository(MiniNavigatorDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все типы с объектами асинхронно
        /// </summary>
        /// <returns>Перечисление типов</returns>
        public async new Task<IEnumerable<ObjectType>> GetAllAsync()
        {
            return await _context.ObjectTypes
                                 .Include(ot => ot.Base).ToListAsync();
        }

        /// <summary>
        /// Получает тип с атрибутами асинхронно
        /// </summary>
        /// <param name="ID">ID типа</param>
        /// <returns>Тип с атрибутами</returns>
        public async Task<ObjectType> GetTypeWithAttributesAsync(Guid? ID)
        {
            return await _context.ObjectTypes
                .Include(ot => ot.Base)
                .Include(ot => ot.Attributes)
                .Include(ot => ot.Attributes.Select(ota => ota.Attribute))
                .FirstOrDefaultAsync(ot => ot.Base_ID == ID);
        }

        /// <summary>
        /// Получает тип с действиями асинхронно
        /// </summary>
        /// <param name="ID">ID типа</param>
        /// <returns>Тип с действиями</returns>
        public async Task<ObjectType> GetTypeWithActionsAsync(Guid? ID)
        {
            return await _context.ObjectTypes
                .Include(ot => ot.Base)
                .Include(ot => ot.Actions)
                .FirstOrDefaultAsync(ot => ot.Base_ID == ID);
        }
    }
}
