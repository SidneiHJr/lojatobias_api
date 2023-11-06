using Ace.Api.Core.Extensions;
using LojaTobias.Api.Core.Extensions;
using LojaTobias.Infra.Data;
using LojaTobias.Infra.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LojaTobias.Financeiro.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<AspnetUserExtension, IdentityRole>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();


            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IApplicationBuilder UseIdentityConfig(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
