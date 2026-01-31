using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public class DeleteVeiculoCommandHandler(
    IVeiculoRepository veiculoRepository) : IRequestHandler<DeleteVeiculoCommand, Result>
{
    public async Task<Result> Handle(DeleteVeiculoCommand request, CancellationToken cancellationToken)
    {
        var veiculo = await veiculoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (veiculo is null)
            return Result.NotFound("Veículo não encontrado");

        await veiculoRepository.DeleteAsync(request.Id, cancellationToken);

        return Result.Success("Veículo removido");
    }
}
