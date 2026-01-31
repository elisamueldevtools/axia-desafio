using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Infra.Context;
using Axia.Veiculos.Infra.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Axia.Veiculos.Infra.Tests.Repositories;

public class UsuarioRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UsuarioRepository _repository;

    public UsuarioRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new UsuarioRepository(_context);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarUsuario()
    {
        //Arrange
        var usuario = Usuario.Create("João", "joao", "hash123", Role.User);

        //Act
        await _repository.AddAsync(usuario);

        //Assert
        var usuarioSalvo = await _context.Usuarios.FindAsync(usuario.Id);
        usuarioSalvo.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByLoginAsync_LoginExistente_DeveRetornarUsuario()
    {
        //Arrange
        var usuario = Usuario.Create("Admin", "admin", "hashAdmin", Role.Admin);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _repository.GetByLoginAsync("admin");

        //Assert
        resultado.Should().NotBeNull();
        resultado!.Role.Should().Be(Role.Admin);
    }

    [Fact]
    public async Task GetByLoginAsync_LoginInexistente_DeveRetornarNull()
    {
        //Act
        var resultado = await _repository.GetByLoginAsync("naoexiste");

        //Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByLoginAsync_LoginExistente_DeveRetornarTrue()
    {
        //Arrange
        var usuario = Usuario.Create("Teste", "existente", "hash", Role.Reader);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        //Act
        var existe = await _repository.ExistsByLoginAsync("existente");

        //Assert
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByLoginAsync_LoginInexistente_DeveRetornarFalse()
    {
        //Act
        var existe = await _repository.ExistsByLoginAsync("naoexiste");

        //Assert
        existe.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
