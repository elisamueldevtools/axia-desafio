using Axia.Veiculos.Application.UseCase.Veiculos.Commands;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Veiculos;

public class UpdateVeiculoCommandHandlerTests
{
    private readonly Mock<IVeiculoRepository> _veiculoRepositoryMock;
    private readonly UpdateVeiculoCommandHandler _handler;

    public UpdateVeiculoCommandHandlerTests()
    {
        _veiculoRepositoryMock = new Mock<IVeiculoRepository>();
        _handler = new UpdateVeiculoCommandHandler(_veiculoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_VeiculoExistente_DeveAtualizar()
    {
        //Arrange
        var id = Guid.NewGuid();
        var veiculo = Veiculo.Create("Antigo", Marca.Fiat, "Uno");
        var command = new UpdateVeiculoCommand(id, "Novo", Marca.Toyota, "Corolla", "Ar, GPS", 120000m);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        _veiculoRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Veiculo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);

        _veiculoRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Veiculo>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_VeiculoNaoEncontrado_DeveRetornarNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();
        var command = new UpdateVeiculoCommand(id, "Desc", Marca.Toyota, "Corolla", null, null);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Veiculo?)null);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);

        _veiculoRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Veiculo>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ComOpcionaisEValor_DeveAtualizarTodosCampos()
    {
        //Arrange
        var id = Guid.NewGuid();
        var veiculo = Veiculo.Create("Antigo", Marca.Chevrolet, "Onix");
        var command = new UpdateVeiculoCommand(id, "SUV Completa", Marca.Jeep, "Compass", "4x4, Couro", 185000m);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        veiculo.Descricao.Should().Be("SUV Completa");
        veiculo.Valor.Should().Be(185000m);
    }
}
