using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Usuarios.Responses;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Queries;

public class ListUsuariosQueryHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<ListUsuariosQuery, Result<IEnumerable<UsuarioResponse>>>
{
    public async Task<Result<IEnumerable<UsuarioResponse>>> Handle(ListUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarios = await usuarioRepository.GetAllAsync(cancellationToken);
        var response = usuarios.Select(UsuarioResponse.FromEntity);

        return Result<IEnumerable<UsuarioResponse>>.Success(response);
    }
}
