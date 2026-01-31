using Axia.Veiculos.Application.Common.Interfaces;
using Axia.Veiculos.Application.UseCase.Usuarios.Commands;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Usuarios;

public class CreateUsuarioCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly CreateUsuarioCommandHandler _handler;

    public CreateUsuarioCommandHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _handler = new CreateUsuarioCommandHandler(
            _usuarioRepositoryMock.Object,
            _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Handle_DadosValidos_DeveCriarUsuario()
    {
        //Arrange
        var command = new CreateUsuarioCommand("João Silva", "joao.silva", "senha123", "User");

        _usuarioRepositoryMock
            .Setup(x => x.ExistsByLoginAsync("joao.silva", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.Hash("senha123"))
            .Returns("hashedPassword");

        _usuarioRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data.Should().NotBeEmpty();

        _usuarioRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Usuario>(u => u.Nome == "João Silva" && u.Login == "joao.silva"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_LoginJaExiste_DeveRetornarBadRequest()
    {
        //Arrange
        var command = new CreateUsuarioCommand("João", "login.existente", "senha123", "User");

        _usuarioRepositoryMock
            .Setup(x => x.ExistsByLoginAsync("login.existente", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
        result.Message.Should().Be("Login já existe");

        _usuarioRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_DeveHashearSenha()
    {
        //Arrange
        var command = new CreateUsuarioCommand("Teste", "teste", "minhasenha", "User");

        _usuarioRepositoryMock
            .Setup(x => x.ExistsByLoginAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.Hash("minhasenha"))
            .Returns("senha_hasheada");

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        _passwordHasherMock.Verify(x => x.Hash("minhasenha"), Times.Once);
        _usuarioRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Usuario>(u => u.SenhaHash == "senha_hasheada"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
