using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Auth;
using Axia.Veiculos.Application.UseCase.Auth.Requests;
using Axia.Veiculos.Application.UseCase.Auth.Responses;
using Axia.Veiculos.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Axia.Veiculos.WebApi.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Login_CredenciaisValidas_DeveRetornar200()
    {
        //Arrange
        var request = new LoginRequest("admin", "admin123");
        var tokenResponse = new TokenResponse("eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...");
        var result = Result<TokenResponse>.Success(tokenResponse, "Login realizado");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.Login(request, CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Login_CredenciaisInvalidas_DeveRetornar401()
    {
        //Arrange
        var request = new LoginRequest("admin", "senhaErrada");
        var result = Result<TokenResponse>.Unauthorized("Login ou senha inválidos");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.Login(request, CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(401);
    }
}
