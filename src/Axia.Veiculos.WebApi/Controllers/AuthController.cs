using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Auth;
using Axia.Veiculos.Application.UseCase.Auth.Requests;
using Axia.Veiculos.Application.UseCase.Auth.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Axia.Veiculos.WebApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Autenticação")]
public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Realizar login")]
    [SwaggerResponse(200, "Sucesso", typeof(ApiResponse<TokenResponse>))]
    [SwaggerResponse(400, "Dados inválidos", typeof(ApiResponse))]
    [SwaggerResponse(401, "Credenciais inválidas", typeof(ApiResponse))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Login, request.Senha);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
