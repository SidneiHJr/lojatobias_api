using LojaTobias.Api.Core;
using LojaTobias.Infra.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Globalization;

namespace LojaTobias.Estoque.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuração para suspender o response de BadRequest do ModelState e permitir o retorno no formato CustomResponse.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddMvc(options =>
            {
                options.MaxModelBindingCollectionSize = int.MaxValue;
            });

            var conn = Environment.GetEnvironmentVariable("DB_CONN") ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(conn);
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                    });

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                         .WithOrigins("http://localhost:4200")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials()
                         .WithExposedHeaders("X-Pagination"));
            });

            services.AddSwaggerGen();

            services.AddEndpointsApiExplorer();

        }

        public static void UseApiConfig(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseIdentityConfig();

            var supportedCultures = new[] { new CultureInfo("pt-BR") };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.MapControllers();
        }

    }
}
