using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Interfaces;
using Axia.Veiculos.Infra.Context;

namespace Axia.Veiculos.Infra.Repositories;

public class VeiculoRepository(AppDbContext context) : BaseRepository<Veiculo>(context), IVeiculoRepository
{
}
