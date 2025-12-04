using MiniNavigator_DB.Context;
using System;
using System.Data.Entity;

namespace MiniNavigator_DB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MiniNavigatorDbContext>());

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MiniNavigatorDbContext>());

            using (var db = new MiniNavigatorDbContext())
            {
                db.Database.Initialize(force: true);
                Console.WriteLine("База данных успешно создана!");
            }
        }
    }
}
