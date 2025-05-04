
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.Providers.Mapping;
using TaskManagementSystem.Repository;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using TaskManagementSystem.Provider.IProvider;
using Microsoft.AspNetCore.SignalR;
using TaskManagementSystem.Provider.Provider;


namespace TaskManagementSystem.Providers
{
    public static class ServicesConfiguration
    {
        public static void AddStudentManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddRepositoryService(configuration);
            services.AddTransient<IAccountProvider,AccountProvider>();
           services.AddTransient<ICommonProvider, CommonProvider>();
            services.AddTransient<ITaskProvider, TaskProvider>();
           services.AddTransient<IAdminProvider, AdminProvider>();
            services.AddTransient<IProjectProvider, ProjectProvider>();
            services.AddTransient<IUserProvider, UserProvider>();
        }
    }
}
