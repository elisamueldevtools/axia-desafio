using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public class UpdateVeiculoCommandHandler(
    IVeiculoRepository veiculoRepository) : IRequestHandler<UpdateVeiculoCommand, Result>
{
    public async Task<Result> Handle(UpdateVeiculoCommand request, CancellationToken cancellationToken)
    {
        var veiculo = await veiculoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (veiculo is null)
            return Result.NotFound("Veículo não encontrado");

        Enum.TryParse<Marca>(request.Marca, ignoreCase: true, out var marca);

        veiculo.Update(request.Descricao, marca, request.Modelo, request.Opcionais, request.Valor);
        await veiculoRepository.UpdateAsync(veiculo, cancellationToken);

        return Result.Success("Veículo atualizado");
    }
}
