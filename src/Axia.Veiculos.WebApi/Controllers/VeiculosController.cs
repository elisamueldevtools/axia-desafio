using Axia.Veiculos.Application.UseCase.Veiculos.Commands;
using Axia.Veiculos.Application.UseCase.Veiculos.Queries;
using Axia.Veiculos.Application.UseCase.Veiculos.Requests;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Axia.Veiculos.WebApi.Controllers;

[Route("api/[controller]")]
public class VeiculosController(IMediator mediator) : BaseController
{
    [HttpGet]
    [AuthorizationAxia(Role.Admin, Role.User, Role.Reader)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ListVeiculosQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [AuthorizationAxia(Role.Admin, Role.User, Role.Reader)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVeiculoByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [AuthorizationAxia(Role.Admin, Role.User)]
    public async Task<IActionResult> Create([FromBody] CreateVeiculoRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateVeiculoCommand(
            request.Descricao,
            request.Marca,
            request.Modelo,
            request.Opcionais,
            request.Valor);

        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [AuthorizationAxia(Role.Admin, Role.User)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVeiculoRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateVeiculoCommand(
            id,
            request.Descricao,
            request.Marca,
            request.Modelo,
            request.Opcionais,
            request.Valor);

        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [AuthorizationAxia(Role.Admin, Role.User)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteVeiculoCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
