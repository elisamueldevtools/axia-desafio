using Axia.Veiculos.Application.UseCase.Usuarios.Commands;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Usuarios;

public class UpdateUsuarioCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly UpdateUsuarioCommandHandler _handler;

    public UpdateUsuarioCommandHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _handler = new UpdateUsuarioCommandHandler(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_UsuarioExistente_DeveAtualizar()
    {
        //Arrange
        var id = Guid.NewGuid();
        var usuario = Usuario.Create("Nome Antigo", "login", "hash", Role.Reader);
        var command = new UpdateUsuarioCommand(id, "Nome Novo", "Admin");

        _usuarioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        _usuarioRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);

        _usuarioRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveRetornarNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();
        var command = new UpdateUsuarioCommand(id, "Nome", "Admin");

        _usuarioRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);

        _usuarioRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
