using Axia.Veiculos.Application.UseCase.Usuarios.Queries;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Usuarios;

public class GetUsuarioByIdQueryHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly GetUsuarioByIdQueryHandler _handler;

    public GetUsuarioByIdQueryHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _handler = new GetUsuarioByIdQueryHandler(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_UsuarioExistente_DeveRetornarUsuario()
    {
        //Arrange
        var id = Guid.NewGuid();
        var usuario = Usuario.Create("João Silva", "joao", "hash", Role.Admin);
        var query = new GetUsuarioByIdQuery(id);

        _usuarioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Data.Should().NotBeNull();
        result.Data!.Nome.Should().Be("João Silva");
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveRetornarNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();
        var query = new GetUsuarioByIdQuery(id);

        _usuarioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.Data.Should().BeNull();
    }
}
