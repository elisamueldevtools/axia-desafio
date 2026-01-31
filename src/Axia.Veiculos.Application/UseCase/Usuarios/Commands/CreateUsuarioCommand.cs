using Axia.Veiculos.Application.Common;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public record CreateUsuarioCommand(string Nome, string Login, string Senha, string Role) : IRequest<Result<Guid>>;
