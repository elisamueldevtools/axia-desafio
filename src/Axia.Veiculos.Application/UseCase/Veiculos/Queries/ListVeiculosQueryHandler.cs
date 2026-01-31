using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Veiculos.Responses;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Queries;

public class ListVeiculosQueryHandler(
    IVeiculoRepository veiculoRepository) : IRequestHandler<ListVeiculosQuery, Result<IEnumerable<VeiculoResponse>>>
{
    public async Task<Result<IEnumerable<VeiculoResponse>>> Handle(ListVeiculosQuery request, CancellationToken cancellationToken)
    {
        var veiculos = await veiculoRepository.GetAllAsync(cancellationToken);
        var response = veiculos.Select(VeiculoResponse.FromEntity);

        return Result<IEnumerable<VeiculoResponse>>.Success(response);
    }
}
