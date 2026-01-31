using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.Common.Interfaces;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public class CreateUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository,
    IPasswordHasher passwordHasher) : IRequestHandler<CreateUsuarioCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (await usuarioRepository.ExistsByLoginAsync(request.Login, cancellationToken))
            return Result<Guid>.BadRequest("Login já existe");

        Enum.TryParse<Role>(request.Role, ignoreCase: true, out var role);

        var senhaHash = passwordHasher.Hash(request.Senha);
        var usuario = Usuario.Create(request.Nome, request.Login, senhaHash, role);

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        return Result<Guid>.Created(usuario.Id, "Usuário criado com sucesso");
    }
}
