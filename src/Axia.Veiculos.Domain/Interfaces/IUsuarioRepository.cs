using Axia.Veiculos.Domain.Entities;

namespace Axia.Veiculos.Domain.Interfaces;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<bool> ExistsByLoginAsync(string login, CancellationToken cancellationToken = default);
}
