using Microsoft.Extensions.DependencyInjection;
using MiniNavigator_Services.Service;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
using System;
using System.Windows.Forms;
using MiniNavigator_Services;

namespace MiniNavigator_UI
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();

            // DI в BLL
            ServiceFactory.AddDependencies(services);

            // Mappers DTO <=> ViewModel

            // Services
            services.AddScoped<IObjectService, ObjectService>();
            services.AddScoped<IObjectTypeService, ObjectTypeService>();

            // Forms
            services.AddScoped<NavigatorForm>();

            // Build
            var provider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var NavigatorForm = provider.GetRequiredService<NavigatorForm>();
            Application.Run(NavigatorForm);
        }
    }
}
