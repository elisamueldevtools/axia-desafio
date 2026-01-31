using Axia.Veiculos.Application.Common;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public record UpdateUsuarioCommand(Guid Id, string Nome, string Role) : IRequest<Result>;
