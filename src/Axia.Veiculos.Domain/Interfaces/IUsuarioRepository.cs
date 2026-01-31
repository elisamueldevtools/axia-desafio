using Axia.Veiculos.Domain.Entities;

namespace Axia.Veiculos.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
