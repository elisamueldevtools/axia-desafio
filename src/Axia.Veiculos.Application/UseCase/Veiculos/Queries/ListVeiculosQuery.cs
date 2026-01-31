using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Veiculos.Responses;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Queries;

public record ListVeiculosQuery : IRequest<Result<IEnumerable<VeiculoResponse>>>;
