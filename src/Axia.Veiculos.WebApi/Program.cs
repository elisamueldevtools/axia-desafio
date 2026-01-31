using Axia.Veiculos.Infra;
using Axia.Veiculos.WebApi.Configurations;
using Axia.Veiculos.WebApi.Extensions;
using Axia.Veiculos.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();

builder.Services
    .AddCustomControllers()
    .AddOpenApi()
    .AddRepositories(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

DependencyInjection.SeedDatabase(app.Services);

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwaggerConfiguration();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
