using MiniNavigator_DB.Context;
namespace MiniNavigator_DB.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MiniNavigatorDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MiniNavigator_DB.Context.MiniNavigatorDbContext";
        }

        protected override void Seed(MiniNavigator_DB.Context.MiniNavigatorDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
