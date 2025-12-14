using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using MiniNavigator_Services;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Service;
using MiniNavigator_Services.Service.Interface;
using MiniNavigator_UI.ViewModel;
using MiniNavigator_UI.Mapper;
using MiniNavigator_UI.Mapper.Interface;

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
            services.AddScoped<IMapper<NavObjectViewModel, NavObjectDTO>, ObjectMapper>();

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
