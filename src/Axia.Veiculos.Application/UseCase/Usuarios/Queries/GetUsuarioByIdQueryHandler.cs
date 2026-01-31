using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Usuarios.Responses;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Queries;

public class GetUsuarioByIdQueryHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<GetUsuarioByIdQuery, Result<UsuarioResponse>>
{
    public async Task<Result<UsuarioResponse>> Handle(GetUsuarioByIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario is null)
            return Result<UsuarioResponse>.NotFound("Usuário não encontrado");

        return Result<UsuarioResponse>.Success(UsuarioResponse.FromEntity(usuario));
    }
}
