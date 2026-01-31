using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Usuarios.Commands;
using Axia.Veiculos.Application.UseCase.Usuarios.Queries;
using Axia.Veiculos.Application.UseCase.Usuarios.Requests;
using Axia.Veiculos.Application.UseCase.Usuarios.Responses;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Axia.Veiculos.WebApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Gerenciamento de usuários (Admin)")]
public class UsuariosController(IMediator mediator) : BaseController
{
    [HttpGet]
    [AuthorizationAxia(Role.Admin)]
    [SwaggerOperation(Summary = "Listar usuários")]
    [SwaggerResponse(200, "Sucesso", typeof(ApiResponse<IEnumerable<UsuarioResponse>>))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ListUsuariosQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [AuthorizationAxia(Role.Admin)]
    [SwaggerOperation(Summary = "Obter usuário por ID")]
    [SwaggerResponse(200, "Sucesso", typeof(ApiResponse<UsuarioResponse>))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    [SwaggerResponse(404, "Não encontrado", typeof(ApiResponse))]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUsuarioByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [AuthorizationAxia(Role.Admin)]
    [SwaggerOperation(Summary = "Cadastrar usuário")]
    [SwaggerResponse(201, "Criado", typeof(ApiResponse<Guid>))]
    [SwaggerResponse(400, "Dados inválidos", typeof(ApiResponse))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUsuarioCommand(request.Nome, request.Login, request.Senha, request.Role);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [AuthorizationAxia(Role.Admin)]
    [SwaggerOperation(Summary = "Atualizar usuário")]
    [SwaggerResponse(200, "Atualizado", typeof(ApiResponse))]
    [SwaggerResponse(400, "Dados inválidos", typeof(ApiResponse))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    [SwaggerResponse(404, "Não encontrado", typeof(ApiResponse))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUsuarioRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUsuarioCommand(id, request.Nome, request.Role);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [AuthorizationAxia(Role.Admin)]
    [SwaggerOperation(Summary = "Remover usuário")]
    [SwaggerResponse(200, "Removido", typeof(ApiResponse))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    [SwaggerResponse(404, "Não encontrado", typeof(ApiResponse))]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteUsuarioCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
