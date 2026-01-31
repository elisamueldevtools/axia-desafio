using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Usuarios.Responses;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Queries;

public record GetUsuarioByIdQuery(Guid Id) : IRequest<Result<UsuarioResponse>>;
