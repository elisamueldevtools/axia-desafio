using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Interfaces;
using Axia.Veiculos.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Axia.Veiculos.Infra.Repositories;

public class VeiculoRepository(AppDbContext context) : IVeiculoRepository
{
    public async Task<Veiculo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Veiculos.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<Veiculo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Veiculos.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Veiculo veiculo, CancellationToken cancellationToken = default)
    {
        await context.Veiculos.AddAsync(veiculo, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Veiculo veiculo, CancellationToken cancellationToken = default)
    {
        context.Veiculos.Update(veiculo);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var veiculo = await GetByIdAsync(id, cancellationToken);
        if (veiculo is not null)
        {
            context.Veiculos.Remove(veiculo);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
