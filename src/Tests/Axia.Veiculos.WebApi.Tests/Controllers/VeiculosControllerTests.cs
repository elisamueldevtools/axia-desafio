using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Application.UseCase.Veiculos.Commands;
using Axia.Veiculos.Application.UseCase.Veiculos.Queries;
using Axia.Veiculos.Application.UseCase.Veiculos.Requests;
using Axia.Veiculos.Application.UseCase.Veiculos.Responses;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Axia.Veiculos.WebApi.Tests.Controllers;

public class VeiculosControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly VeiculosController _controller;

    public VeiculosControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new VeiculosController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaDeVeiculos()
    {
        //Arrange
        var veiculos = new List<VeiculoResponse>
        {
            new(Guid.NewGuid(), "Sedan", "Toyota", "Corolla", "Ar", 100000m),
            new(Guid.NewGuid(), "Hatch", "Fiat", "Uno", null, 35000m)
        };
        var result = Result<IEnumerable<VeiculoResponse>>.Success(veiculos);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ListVeiculosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.GetAll(CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_VeiculoExistente_DeveRetornar200()
    {
        //Arrange
        var id = Guid.NewGuid();
        var veiculo = new VeiculoResponse(id, "Sedan", "Toyota", "Corolla", "Ar", 100000m);
        var result = Result<VeiculoResponse>.Success(veiculo);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetVeiculoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.GetById(id, CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_VeiculoInexistente_DeveRetornar404()
    {
        //Arrange
        var result = Result<VeiculoResponse>.NotFound("Veículo não encontrado");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetVeiculoByIdQuery>(), It.IsAny<CancellationToken>()))
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
        var request = new CreateVeiculoRequest("Sedan", Marca.Toyota, "Corolla", "Ar", 100000m);
        var id = Guid.NewGuid();
        var result = Result<Guid>.Created(id, "Veículo cadastrado");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateVeiculoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.Create(request, CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult!.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Delete_VeiculoInexistente_DeveRetornar404()
    {
        //Arrange
        var result = Result.NotFound("Veículo não encontrado");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<DeleteVeiculoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        //Act
        var actionResult = await _controller.Delete(Guid.NewGuid(), CancellationToken.None);

        //Assert
        var objectResult = actionResult as ObjectResult;
        objectResult!.StatusCode.Should().Be(404);
    }
}
