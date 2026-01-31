using Axia.Veiculos.Application.Common.Interfaces;
using Axia.Veiculos.Application.UseCase.Auth;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        _handler = new LoginCommandHandler(
            _usuarioRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task Handle_CredenciaisValidas_DeveRetornarToken()
    {
        //Arrange
        var command = new LoginCommand("admin", "admin123");
        var usuario = Usuario.Create("Admin", "admin", "hashedPassword", Role.Admin);
        var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...";

        _usuarioRepositoryMock
            .Setup(x => x.GetByLoginAsync("admin", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        _passwordHasherMock
            .Setup(x => x.Verify("admin123", "hashedPassword"))
            .Returns(true);

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(usuario))
            .Returns(token);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Data.Should().NotBeNull();
        result.Data!.AccessToken.Should().Be(token);
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveRetornarUnauthorized()
    {
        //Arrange
        var command = new LoginCommand("inexistente", "senha");

        _usuarioRepositoryMock
            .Setup(x => x.GetByLoginAsync("inexistente", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task Handle_SenhaIncorreta_DeveRetornarUnauthorized()
    {
        //Arrange
        var command = new LoginCommand("admin", "senhaErrada");
        var usuario = Usuario.Create("Admin", "admin", "hashedPassword", Role.Admin);

        _usuarioRepositoryMock
            .Setup(x => x.GetByLoginAsync("admin", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        _passwordHasherMock
            .Setup(x => x.Verify("senhaErrada", "hashedPassword"))
            .Returns(false);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(401);
        _jwtTokenGeneratorMock.Verify(x => x.GenerateToken(It.IsAny<Usuario>()), Times.Never);
    }
}
