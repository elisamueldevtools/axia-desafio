using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Veiculos.Commands;
using Axia.Veiculos.Application.UseCase.Veiculos.Queries;
using Axia.Veiculos.Application.UseCase.Veiculos.Requests;
using Axia.Veiculos.Application.UseCase.Veiculos.Responses;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Axia.Veiculos.WebApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Gerenciamento de veículos")]
public class VeiculosController(IMediator mediator) : BaseController
{
    [HttpGet]
    [AuthorizationAxia(Role.Admin, Role.User, Role.Reader)]
    [SwaggerOperation(Summary = "Listar veículos")]
    [SwaggerResponse(200, "Sucesso", typeof(ApiResponse<IEnumerable<VeiculoResponse>>))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ListVeiculosQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [AuthorizationAxia(Role.Admin, Role.User, Role.Reader)]
    [SwaggerOperation(Summary = "Obter veículo por ID")]
    [SwaggerResponse(200, "Sucesso", typeof(ApiResponse<VeiculoResponse>))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    [SwaggerResponse(404, "Não encontrado", typeof(ApiResponse))]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVeiculoByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [AuthorizationAxia(Role.Admin, Role.User)]
    [SwaggerOperation(Summary = "Cadastrar veículo")]
    [SwaggerResponse(201, "Criado", typeof(ApiResponse<Guid>))]
    [SwaggerResponse(400, "Dados inválidos", typeof(ApiResponse))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
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
    [SwaggerOperation(Summary = "Atualizar veículo")]
    [SwaggerResponse(200, "Atualizado", typeof(ApiResponse))]
    [SwaggerResponse(400, "Dados inválidos", typeof(ApiResponse))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    [SwaggerResponse(404, "Não encontrado", typeof(ApiResponse))]
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
    [SwaggerOperation(Summary = "Remover veículo")]
    [SwaggerResponse(200, "Removido", typeof(ApiResponse))]
    [SwaggerResponse(401, "Token inválido", typeof(ApiResponse))]
    [SwaggerResponse(403, "Sem permissão", typeof(ApiResponse))]
    [SwaggerResponse(404, "Não encontrado", typeof(ApiResponse))]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteVeiculoCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
