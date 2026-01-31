using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Usuarios.Commands;
using Axia.Veiculos.Application.UseCase.Usuarios.Queries;
using Axia.Veiculos.Application.UseCase.Usuarios.Requests;
using Axia.Veiculos.Application.UseCase.Usuarios.Responses;
using Axia.Veiculos.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Axia.Veiculos.WebApi.Tests.Controllers;

public class UsuariosControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UsuariosController _controller;

    public UsuariosControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsuariosController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaDeUsuarios()
    {
        //Arrange
        var usuarios = new List<UsuarioResponse>
        {
            new(Guid.NewGuid(), "Admin", "admin", "Admin"),
            new(Guid.NewGuid(), "User", "user", "User")
        };
        var result = Result<IEnumerable<UsuarioResponse>>.Success(usuarios);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ListUsuariosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.GetAll(CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_UsuarioExistente_DeveRetornar200()
    {
        //Arrange
        var id = Guid.NewGuid();
        var usuario = new UsuarioResponse(id, "Admin", "admin", "Admin");
        var result = Result<UsuarioResponse>.Success(usuario);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetUsuarioByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.GetById(id, CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_UsuarioInexistente_DeveRetornar404()
    {
        //Arrange
        var result = Result<UsuarioResponse>.NotFound("Usuário não encontrado");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetUsuarioByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.GetById(Guid.NewGuid(), CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Create_DadosValidos_DeveRetornar201()
    {
        //Arrange
        var request = new CreateUsuarioRequest("João", "joao", "senha123", "User");
        var id = Guid.NewGuid();
        var result = Result<Guid>.Created(id, "Usuário criado");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateUsuarioCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.Create(request, CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult!.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Delete_UsuarioInexistente_DeveRetornar404()
    {
        //Arrange
        var result = Result.NotFound("Usuário não encontrado");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<DeleteUsuarioCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.Delete(Guid.NewGuid(), CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult!.StatusCode.Should().Be(404);
    }
}
