using Axia.Veiculos.Application.UseCase.Veiculos.Queries;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Veiculos;

public class ListVeiculosQueryHandlerTests
{
    private readonly Mock<IVeiculoRepository> _veiculoRepositoryMock;
    private readonly ListVeiculosQueryHandler _handler;

    public ListVeiculosQueryHandlerTests()
    {
        _veiculoRepositoryMock = new Mock<IVeiculoRepository>();
        _handler = new ListVeiculosQueryHandler(_veiculoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ComVeiculos_DeveRetornarLista()
    {
        //Arrange
        var veiculos = new List<Veiculo>
        {
            Veiculo.Create("Sedan", Marca.Toyota, "Corolla", "Ar", 100000m),
            Veiculo.Create("Hatch", Marca.Fiat, "Uno", null, 35000m)
        };

        _veiculoRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculos);

        //Act
        var result = await _handler.Handle(new ListVeiculosQuery(), CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_SemVeiculos_DeveRetornarListaVazia()
    {
        //Arrange
        _veiculoRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Veiculo>());

        //Act
        var result = await _handler.Handle(new ListVeiculosQuery(), CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}
