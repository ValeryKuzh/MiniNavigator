using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MiniNavigator_DB
{
    public class MiniNavigatorDbConfiguration : DbConfiguration
    {
        public MiniNavigatorDbConfiguration()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}
