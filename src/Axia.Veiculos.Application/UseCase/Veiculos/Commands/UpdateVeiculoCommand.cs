using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Domain.Enums;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public record UpdateVeiculoCommand(Guid Id, string Descricao, Marca Marca, string Modelo, string? Opcionais, decimal? Valor) : IRequest<Result>;
