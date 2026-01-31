using Axia.Veiculos.Application.UseCase.Veiculos.Commands;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Veiculos;

public class DeleteVeiculoCommandHandlerTests
{
    private readonly Mock<IVeiculoRepository> _veiculoRepositoryMock;
    private readonly DeleteVeiculoCommandHandler _handler;

    public DeleteVeiculoCommandHandlerTests()
    {
        _veiculoRepositoryMock = new Mock<IVeiculoRepository>();
        _handler = new DeleteVeiculoCommandHandler(_veiculoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_VeiculoExistente_DeveRemover()
    {
        //Arrange
        var id = Guid.NewGuid();
        var veiculo = Veiculo.Create("Sedan", Marca.Toyota, "Corolla");
        var command = new DeleteVeiculoCommand(id);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        _veiculoRepositoryMock
            .Setup(x => x.DeleteAsync(id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);

        _veiculoRepositoryMock.Verify(
            x => x.DeleteAsync(id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_VeiculoNaoEncontrado_DeveRetornarNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();
        var command = new DeleteVeiculoCommand(id);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Veiculo?)null);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);

        _veiculoRepositoryMock.Verify(
            x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
