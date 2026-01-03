using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MiniNavigator_DB
{
    /// <summary>
    /// Класс конфигурации для работы с базой данных
    /// </summary>
    public class MiniNavigatorDbConfiguration : DbConfiguration
    {
        public MiniNavigatorDbConfiguration()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}
