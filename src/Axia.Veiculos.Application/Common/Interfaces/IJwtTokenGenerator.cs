using Axia.Veiculos.Domain.Entities;

namespace Axia.Veiculos.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Usuario usuario);
}
