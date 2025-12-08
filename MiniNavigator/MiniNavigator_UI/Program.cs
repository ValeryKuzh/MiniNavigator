using Microsoft.Extensions.DependencyInjection;
using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.Service;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper;
using MiniNavigator_Services.Mapper.Interface;
using MiniNavigator_Services.Service.Interface;
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

            // DbContext 
            services.AddScoped<MiniNavigatorDbContext>(sp => new MiniNavigatorDbContext());

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
            services.AddScoped<IObjectTypeRepository, ObjectTypeRepository>();

            // Mappers Entity <=> DTO
            services.AddScoped<IMapper<NavObjectDTO, BaseObject>, ObjectMapper>();
            services.AddScoped<IMapper<ObjectTypeDTO, ObjectType>, ObjectTypeMapper>();
            services.AddScoped<IMapper<ObjectActionDTO, ObjectAction>, ObjectActionMapper>();

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
