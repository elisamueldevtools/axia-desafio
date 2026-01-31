using Axia.Veiculos.Application.Common;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public record CreateVeiculoCommand(string Descricao, string Marca, string Modelo, string? Opcionais, decimal? Valor) : IRequest<Result<Guid>>;
