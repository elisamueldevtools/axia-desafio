using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public class UpdateUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<UpdateUsuarioCommand, Result>
{
    public async Task<Result> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario is null)
            return Result.NotFound("Usuário não encontrado");

        Enum.TryParse<Role>(request.Role, ignoreCase: true, out var role);

        usuario.Update(request.Nome, role);
        await usuarioRepository.UpdateAsync(usuario, cancellationToken);

        return Result.Success("Usuário atualizado");
    }
}
