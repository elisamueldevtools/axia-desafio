using Axia.Veiculos.Application.UseCase.Usuarios.Queries;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Usuarios;

public class ListUsuariosQueryHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly ListUsuariosQueryHandler _handler;

    public ListUsuariosQueryHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _handler = new ListUsuariosQueryHandler(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ComUsuarios_DeveRetornarLista()
    {
        //Arrange
        var usuarios = new List<Usuario>
        {
            Usuario.Create("Admin", "admin", "hash1", Role.Admin),
            Usuario.Create("User", "user", "hash2", Role.User)
        };

        _usuarioRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuarios);

        //Act
        var result = await _handler.Handle(new ListUsuariosQuery(), CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_SemUsuarios_DeveRetornarListaVazia()
    {
        //Arrange
        _usuarioRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Usuario>());

        //Act
        var result = await _handler.Handle(new ListUsuariosQuery(), CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}
