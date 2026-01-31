using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Axia.Veiculos.Application.Common.Interfaces;
using Axia.Veiculos.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Axia.Veiculos.Infra.Security;

public class JwtTokenGenerator(IConfiguration configuration, RsaKeyService rsaKeyService) : IJwtTokenGenerator
{
    public string GenerateToken(Usuario usuario)
    {
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"]!);

        var credentials = rsaKeyService.GetSigningCredentials();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, usuario.Login),
            new(JwtRegisteredClaimNames.Name, usuario.Nome),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, usuario.Role.ToString()),
            new("role", usuario.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
