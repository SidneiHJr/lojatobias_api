using LojaTobias.Identidade.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure(app);

app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddEndpointsApiExplorer();
    services.AddDependencyConfig();
    services.AddSwaggerConfig();
    services.AddIdentityConfig(configuration);
    services.AddApiConfig(configuration);
}

void Configure(WebApplication app)
{
    app.UseSwaggerConfig();

    app.UseApiConfig();

}
