using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Veiculos.Responses;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Queries;

public record GetVeiculoByIdQuery(Guid Id) : IRequest<Result<VeiculoResponse>>;
