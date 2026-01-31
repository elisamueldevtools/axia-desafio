namespace Axia.Veiculos.Application.UseCase.Veiculos.Requests;

public record UpdateVeiculoRequest(string Descricao, string Marca, string Modelo, string? Opcionais, decimal? Valor);
