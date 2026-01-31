using Axia.Veiculos.Application.Common.Interfaces;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using Axia.Veiculos.Infra.Context;
using Axia.Veiculos.Infra.Repositories;
using Axia.Veiculos.Infra.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Axia.Veiculos.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("AxiaVeiculosDb"));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        var rsaKeyService = new RsaKeyService(configuration);
        services.AddSingleton(rsaKeyService);
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            var issuer = configuration["Jwt:Issuer"]!;
            var audience = configuration["Jwt:Audience"]!;

            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = rsaKeyService.PublicKey,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = System.Security.Claims.ClaimTypes.Role
            };
        });

        services.AddAuthorization();

        return services;
    }

    public static void SeedDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if (!context.Usuarios.Any())
        {
            var admin = Usuario.Create("Administrador", "admin", passwordHasher.Hash("admin1234"), Role.Admin);
            context.Usuarios.Add(admin);
            context.SaveChanges();
        }
    }
}
