using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Auth.Responses;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Auth;

public record LoginCommand(string Login, string Senha) : IRequest<Result<TokenResponse>>;
