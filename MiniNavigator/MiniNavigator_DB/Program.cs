using MiniNavigator_DB.Configuration;
using MiniNavigator_DB.Context;
using System;
using System.Data.Entity;

namespace MiniNavigator_DB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MiniNavigatorDbContext())
            {
                Database.SetInitializer(new MiniNavigatorDbInitializer());

                db.Database.Initialize(force: true);
                Console.WriteLine("База данных успешно создана!");
            }
            //using (var db = new MiniNavigatorDbContext())
            //{
            //    db.Database.Delete(); // Удаляет БД
            //}
        }
    }
}
