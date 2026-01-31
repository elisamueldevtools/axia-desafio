using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Interfaces;
using Axia.Veiculos.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Axia.Veiculos.Infra.Repositories;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Usuarios.FindAsync([id], cancellationToken);
    }

    public async Task<Usuario?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await context.Usuarios.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Usuarios.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await context.Usuarios.AnyAsync(u => u.Login == login, cancellationToken);
    }

    public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await context.Usuarios.AddAsync(usuario, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        context.Usuarios.Update(usuario);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await GetByIdAsync(id, cancellationToken);
        if (usuario is not null)
        {
            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
