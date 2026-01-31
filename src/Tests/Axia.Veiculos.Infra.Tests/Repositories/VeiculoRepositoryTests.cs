using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Infra.Context;
using Axia.Veiculos.Infra.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Axia.Veiculos.Infra.Tests.Repositories;

public class VeiculoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly VeiculoRepository _repository;

    public VeiculoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new VeiculoRepository(_context);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarVeiculo()
    {
        //Arrange
        var veiculo = Veiculo.Create("Sedan", Marca.Toyota, "Corolla");

        //Act
        await _repository.AddAsync(veiculo);

        //Assert
        var veiculoSalvo = await _context.Veiculos.FindAsync(veiculo.Id);
        veiculoSalvo.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_VeiculoExistente_DeveRetornarVeiculo()
    {
        //Arrange
        var veiculo = Veiculo.Create("SUV", Marca.Jeep, "Compass");
        await _context.Veiculos.AddAsync(veiculo);
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _repository.GetByIdAsync(veiculo.Id);

        //Assert
        resultado.Should().NotBeNull();
        resultado!.Modelo.Should().Be("Compass");
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosVeiculos()
    {
        //Arrange
        await _context.Veiculos.AddRangeAsync(
            Veiculo.Create("V1", Marca.Honda, "Civic"),
            Veiculo.Create("V2", Marca.Fiat, "Uno"));
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _repository.GetAllAsync();

        //Assert
        resultado.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteAsync_VeiculoExistente_DeveRemover()
    {
        //Arrange
        var veiculo = Veiculo.Create("ParaDeletar", Marca.Renault, "Sandero");
        await _context.Veiculos.AddAsync(veiculo);
        await _context.SaveChangesAsync();

        //Act
        await _repository.DeleteAsync(veiculo.Id);

        //Assert
        var veiculoDeletado = await _context.Veiculos.FindAsync(veiculo.Id);
        veiculoDeletado.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
