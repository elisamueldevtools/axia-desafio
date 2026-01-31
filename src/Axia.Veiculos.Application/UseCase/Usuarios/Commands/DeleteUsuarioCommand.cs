using Axia.Veiculos.Application.Common;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public record DeleteUsuarioCommand(Guid Id) : IRequest<Result>;
