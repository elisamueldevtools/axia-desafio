using Axia.Veiculos.Application.UseCase.Veiculos.Queries;
using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Axia.Veiculos.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Axia.Veiculos.Application.Tests.UseCase.Veiculos;

public class GetVeiculoByIdQueryHandlerTests
{
    private readonly Mock<IVeiculoRepository> _veiculoRepositoryMock;
    private readonly GetVeiculoByIdQueryHandler _handler;

    public GetVeiculoByIdQueryHandlerTests()
    {
        _veiculoRepositoryMock = new Mock<IVeiculoRepository>();
        _handler = new GetVeiculoByIdQueryHandler(_veiculoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_VeiculoExistente_DeveRetornarVeiculo()
    {
        //Arrange
        var id = Guid.NewGuid();
        var veiculo = Veiculo.Create("Sedan Completo", Marca.Toyota, "Corolla", "Ar, GPS", 125000m);
        var query = new GetVeiculoByIdQuery(id);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Data.Should().NotBeNull();
        result.Data!.Descricao.Should().Be("Sedan Completo");
    }

    [Fact]
    public async Task Handle_VeiculoNaoEncontrado_DeveRetornarNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();
        var query = new GetVeiculoByIdQuery(id);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Veiculo?)null);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.Message.Should().Be("Veículo não encontrado");
    }

    [Fact]
    public async Task Handle_DeveMapearParaResponse()
    {
        //Arrange
        var veiculo = Veiculo.Create("SUV", Marca.Jeep, "Compass", "4x4", 180000m);
        var query = new GetVeiculoByIdQuery(veiculo.Id);

        _veiculoRepositoryMock
            .Setup(x => x.GetByIdAsync(veiculo.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.Data!.Id.Should().Be(veiculo.Id);
        result.Data.Marca.Should().Be(veiculo.Marca.ToString());
    }
}
