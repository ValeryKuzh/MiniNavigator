using MiniNavigator_DB.Context;

namespace MiniNavigator_DB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MiniNavigatorDbContext())
            {
                context.Database.Initialize(true);
            }
        }
    }
}
