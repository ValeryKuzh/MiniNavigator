using Microsoft.Extensions.DependencyInjection;
using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper;
using MiniNavigator_Services.Mapper.Interface;

namespace MiniNavigator_Services
{
    public static class ServiceFactory
    {
        public static void AddDependencies(IServiceCollection services)
        {
            // DbContext 
            services.AddScoped<MiniNavigatorDbContext>(sp => new MiniNavigatorDbContext());

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
            services.AddScoped<IObjectTypeRepository, ObjectTypeRepository>();
            services.AddScoped<IObjectAttributeRepository, ObjectAttributeRepository>();
            services.AddScoped<IObjectAttributeValueRepository, ObjectAttributeValueRepository>();

            // Mappers Entity <=> DTO
            services.AddScoped<IMapper<NavObjectDTO, BaseObject>, ObjectMapper>();
            services.AddScoped<IMapper<ObjectTypeDTO, ObjectType>, ObjectTypeMapper>();
            services.AddScoped<IMapper<ObjectActionDTO, ObjectAction>, ObjectActionMapper>();
            services.AddScoped<IMapper<ObjectAttributeDTO, ObjectAttributeValue>, AttributeMapper>();
        }
    }
}
