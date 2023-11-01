using LojaTobias.Api.Core.Extensions;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Notifications;
using LojaTobias.Domain.Services;
using LojaTobias.Infra.Data;
using LojaTobias.Infra.Services;

namespace LojaTobias.Catalogo.Api.Configuration
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(Service<>));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<ILogProvider, LogProvider>();

            services.AddScoped<INotifiable, Notifiable>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAspnetUser, AspnetUser>();

            return services;
        }

    }
}
