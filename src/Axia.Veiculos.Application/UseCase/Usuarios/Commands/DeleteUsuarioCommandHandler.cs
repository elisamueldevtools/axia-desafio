using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public class DeleteUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<DeleteUsuarioCommand, Result>
{
    public async Task<Result> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario is null)
            return Result.NotFound("Usuário não encontrado");

        await usuarioRepository.DeleteAsync(request.Id, cancellationToken);

        return Result.Success("Usuário removido");
    }
}
