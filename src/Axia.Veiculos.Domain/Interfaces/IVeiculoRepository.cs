using Axia.Veiculos.Domain.Entities;

namespace Axia.Veiculos.Domain.Interfaces;

public interface IVeiculoRepository
{
    Task<Veiculo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Veiculo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Veiculo veiculo, CancellationToken cancellationToken = default);
    Task UpdateAsync(Veiculo veiculo, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
