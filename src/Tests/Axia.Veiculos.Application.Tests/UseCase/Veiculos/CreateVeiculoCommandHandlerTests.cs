using Axia.Veiculos.Application.UseCase.Veiculos.Commands;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Veiculos;

public class CreateVeiculoCommandHandlerTests
{
    private readonly Mock<IVeiculoRepository> _veiculoRepositoryMock;
    private readonly CreateVeiculoCommandHandler _handler;

    public CreateVeiculoCommandHandlerTests()
    {
        _veiculoRepositoryMock = new Mock<IVeiculoRepository>();
        _handler = new CreateVeiculoCommandHandler(_veiculoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_DadosValidos_DeveCriarVeiculo()
    {
        //Arrange
        var command = new CreateVeiculoCommand(
            "Sedan Completo",
            Marca.Toyota,
            "Corolla",
            "Ar condicionado, GPS",
            125000m);

        _veiculoRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Veiculo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data.Should().NotBeEmpty();

        _veiculoRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Veiculo>(v =>
                v.Descricao == "Sedan Completo" &&
                v.Modelo == "Corolla"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SemOpcionais_DeveCriarVeiculoComOpcionaisNulo()
    {
        //Arrange
        var command = new CreateVeiculoCommand(
            "Hatch Básico",
            Marca.Fiat,
            "Uno",
            null,
            35000m);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        _veiculoRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Veiculo>(v => v.Opcionais == null), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
