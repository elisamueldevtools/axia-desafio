using Axia.Veiculos.Domain.Entities;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Responses;

public record UsuarioResponse(Guid Id, string Nome, string Login, string Role)
{
    public static UsuarioResponse FromEntity(Usuario usuario)
        => new(usuario.Id, usuario.Nome, usuario.Login, usuario.Role.ToString());
}
