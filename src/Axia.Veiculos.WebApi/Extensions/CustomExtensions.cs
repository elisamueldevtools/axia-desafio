using System.Text.Json.Serialization;
using Axia.Veiculos.Application;
using Axia.Veiculos.Infra;
using Axia.Veiculos.WebApi.Configurations;
using Serilog;

namespace Axia.Veiculos.WebApi.Extensions;

public static class CustomExtensions
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddSwaggerConfiguration();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddApplication();
        return services;
    }

    public static IHostBuilder AddSerilog(this IHostBuilder host)
    {
        host.UseSerilog((ctx, lc) => lc
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console());

        return host;
    }
}
