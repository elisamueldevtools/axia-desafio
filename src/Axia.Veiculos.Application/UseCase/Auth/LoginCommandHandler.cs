using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.Common.Interfaces;
using Axia.Veiculos.Application.UseCase.Auth.Responses;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Auth;

public class LoginCommandHandler(
    IUsuarioRepository usuarioRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByLoginAsync(request.Login, cancellationToken);

        if (usuario is null || !passwordHasher.Verify(request.Senha, usuario.SenhaHash))
            return Result<TokenResponse>.Unauthorized("Login ou senha inválidos");

        var token = jwtTokenGenerator.GenerateToken(usuario);
        var response = new TokenResponse(token);

        return Result<TokenResponse>.Success(response, "Login realizado com sucesso");
    }
}
