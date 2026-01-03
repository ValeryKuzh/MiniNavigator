using Microsoft.Extensions.DependencyInjection;
using MiniNavigator_Services;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Service;
using MiniNavigator_Services.Service.Interface;
using MiniNavigator_UI.Mapper;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.Service;
using MiniNavigator_UI.Service.ActionHandler;
using MiniNavigator_UI.Service.ActionHandler.Handler;
using MiniNavigator_UI.ViewModel;
using System;
using System.Windows.Forms;

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
            services.AddScoped<IMapper<NavObjectViewModel, NavObjectDTO>, ObjectMapper>();

            // Services
            services.AddScoped<IObjectService, ObjectService>();
            services.AddScoped<IObjectTypeService, ObjectTypeService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAttributeService, AttributeService>();
            services.AddScoped<IValidationService, ValidationService>();

            // Action handlers
            services.AddSingleton<IObjectActionHandler, EditActionHandler>();
            services.AddSingleton<IObjectActionHandler, DeleteActionHandler>();
            services.AddSingleton<IObjectActionHandler, DownloadActionHandler>();
            services.AddSingleton<IObjectActionHandler, OpenActionHandler>();

            // Registry
            services.AddSingleton<ActionHandlerRegistry>(sp =>
            {
                var handlers = sp.GetServices<IObjectActionHandler>();
                return new ActionHandlerRegistry(handlers);
            });

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
