using Axia.Veiculos.Application.UseCase.Auth;
using Axia.Veiculos.Application.UseCase.Auth.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Axia.Veiculos.WebApi.Controllers;

[Route("api/[controller]")]
public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Login, request.Senha);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
