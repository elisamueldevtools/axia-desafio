using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public class CreateVeiculoCommandHandler(
    IVeiculoRepository veiculoRepository) : IRequestHandler<CreateVeiculoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateVeiculoCommand request, CancellationToken cancellationToken)
    {
        Enum.TryParse<Marca>(request.Marca, ignoreCase: true, out var marca);

        var veiculo = Veiculo.Create(request.Descricao, marca, request.Modelo, request.Opcionais, request.Valor);

        await veiculoRepository.AddAsync(veiculo, cancellationToken);

        return Result<Guid>.Created(veiculo.Id, "Veículo cadastrado");
    }
}
