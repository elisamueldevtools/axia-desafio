using Axia.Veiculos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Axia.Veiculos.Infra.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Login).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SenhaHash).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Role).HasConversion<int>().IsRequired();
            entity.HasIndex(e => e.Login).IsUnique();
        });

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Marca).HasConversion<int>().IsRequired();
            entity.Property(e => e.Modelo).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Opcionais).HasMaxLength(500);
            entity.Property(e => e.Valor).HasPrecision(18, 2);
        });

        base.OnModelCreating(modelBuilder);
    }
}
