namespace Axia.Veiculos.Application.UseCase.Veiculos.Requests;

public record CreateVeiculoRequest(string Descricao, string Marca, string Modelo, string? Opcionais, decimal? Valor);
