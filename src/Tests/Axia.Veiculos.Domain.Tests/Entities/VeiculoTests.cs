using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Xunit;

namespace Axia.Veiculos.Domain.Tests.Entities;

public class VeiculoTests
{
    [Fact]
    public void Create_DeveRetornarVeiculoComDadosCorretos()
    {
        //Arrange
        var descricao = "Sedan Completo";
        var marca = Marca.Toyota;
        var modelo = "Corolla";

        //Act
        var veiculo = Veiculo.Create(descricao, marca, modelo, "Ar, Direção", 125000m);

        //Assert
        Assert.NotEqual(Guid.Empty, veiculo.Id);
        Assert.Equal(descricao, veiculo.Descricao);
        Assert.Equal(marca, veiculo.Marca);
        Assert.Equal(modelo, veiculo.Modelo);
        Assert.Equal("Ar, Direção", veiculo.Opcionais);
        Assert.Equal(125000m, veiculo.Valor);
    }

    [Fact]
    public void Create_SemOpcionais_DevePermitirNulos()
    {
        //Arrange & Act
        var veiculo = Veiculo.Create("Descricao", Marca.Fiat, "Uno");

        //Assert
        Assert.Null(veiculo.Opcionais);
        Assert.Null(veiculo.Valor);
    }

    [Fact]
    public void Update_DeveAtualizarCampos()
    {
        //Arrange
        var veiculo = Veiculo.Create("Antigo", Marca.Ford, "Ka");
        var idOriginal = veiculo.Id;

        //Act
        veiculo.Update("Novo", Marca.Chevrolet, "Onix", "Ar, GPS", 75000m);

        //Assert
        Assert.Equal(idOriginal, veiculo.Id); // id nao muda
        Assert.Equal("Novo", veiculo.Descricao);
        Assert.Equal(Marca.Chevrolet, veiculo.Marca);
        Assert.Equal("Onix", veiculo.Modelo);
    }
}
