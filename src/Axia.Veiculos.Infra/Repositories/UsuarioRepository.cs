using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Interfaces;
using Axia.Veiculos.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Axia.Veiculos.Infra.Repositories;

public class UsuarioRepository(AppDbContext context) : BaseRepository<Usuario>(context), IUsuarioRepository
{
    public async Task<Usuario?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public async Task<bool> ExistsByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(u => u.Login == login, cancellationToken);
    }
}
