using Axia.Veiculos.Application.UseCase.Usuarios.Commands;
using Axia.Veiculos.Application.UseCase.Usuarios.Queries;
using Axia.Veiculos.Application.UseCase.Usuarios.Requests;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Axia.Veiculos.WebApi.Controllers;

[Route("api/[controller]")]
public class UsuariosController(IMediator mediator) : BaseController
{
    [HttpGet]
    [AuthorizationAxia(Role.Admin)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ListUsuariosQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [AuthorizationAxia(Role.Admin)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUsuarioByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [AuthorizationAxia(Role.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUsuarioCommand(request.Nome, request.Login, request.Senha, request.Role);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [AuthorizationAxia(Role.Admin)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUsuarioRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUsuarioCommand(id, request.Nome, request.Role);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [AuthorizationAxia(Role.Admin)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteUsuarioCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
