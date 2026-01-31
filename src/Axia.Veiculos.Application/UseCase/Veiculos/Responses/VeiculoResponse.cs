using Axia.Veiculos.Domain.Entities;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Responses;

public record VeiculoResponse(Guid Id, string Descricao, string Marca, string Modelo, string? Opcionais, decimal? Valor)
{
    public static VeiculoResponse FromEntity(Veiculo veiculo)
        => new(veiculo.Id, veiculo.Descricao, veiculo.Marca.ToString(), veiculo.Modelo, veiculo.Opcionais, veiculo.Valor);
}
