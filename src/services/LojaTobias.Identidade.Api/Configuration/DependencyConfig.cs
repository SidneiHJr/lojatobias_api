using LojaTobias.Api.Core.Extensions;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Notifications;
using LojaTobias.Domain.Services;
using LojaTobias.Infra.Data;

namespace LojaTobias.Identidade.Api.Configuration
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<INotifiable, Notifiable>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAspnetUser, AspnetUser>();

            return services;
        }

    }
}
