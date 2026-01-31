using Axia.Veiculos.Application.Common;
using MediatR;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public record DeleteVeiculoCommand(Guid Id) : IRequest<Result>;
