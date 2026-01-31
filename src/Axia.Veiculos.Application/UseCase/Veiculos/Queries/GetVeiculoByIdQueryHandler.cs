using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Veiculos.Responses;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Queries;

public class GetVeiculoByIdQueryHandler(
    IVeiculoRepository veiculoRepository) : IRequestHandler<GetVeiculoByIdQuery, Result<VeiculoResponse>>
{
    public async Task<Result<VeiculoResponse>> Handle(GetVeiculoByIdQuery request, CancellationToken cancellationToken)
    {
        var veiculo = await veiculoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (veiculo is null)
            return Result<VeiculoResponse>.NotFound("Veículo não encontrado");

        return Result<VeiculoResponse>.Success(VeiculoResponse.FromEntity(veiculo));
    }
}
