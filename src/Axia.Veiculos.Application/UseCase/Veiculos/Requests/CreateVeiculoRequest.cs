using Axia.Veiculos.Domain.Enums;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Requests;

public record CreateVeiculoRequest(string Descricao, Marca Marca, string Modelo, string? Opcionais, decimal? Valor);
