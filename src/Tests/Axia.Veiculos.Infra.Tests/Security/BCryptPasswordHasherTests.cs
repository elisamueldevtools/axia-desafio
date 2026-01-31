using Axia.Veiculos.Infra.Security;
using FluentAssertions;
using Xunit;

namespace Axia.Veiculos.Infra.Tests.Security;

public class BCryptPasswordHasherTests
{
    private readonly BCryptPasswordHasher _hasher;

    public BCryptPasswordHasherTests()
    {
        _hasher = new BCryptPasswordHasher();
    }

    [Fact]
    public void Hash_DeveRetornarHashDiferenteDaSenha()
    {
        //Arrange
        var senha = "minhaSenha123";

        //Act
        var hash = _hasher.Hash(senha);

        //Assert
        hash.Should().NotBe(senha);
        hash.Should().StartWith("$2");
    }

    [Fact]
    public void Verify_SenhaCorreta_DeveRetornarTrue()
    {
        //Arrange
        var senha = "senhaSecreta";
        var hash = _hasher.Hash(senha);

        //Act
        var resultado = _hasher.Verify(senha, hash);

        //Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void Verify_SenhaIncorreta_DeveRetornarFalse()
    {
        //Arrange
        var senha = "senhaCorreta";
        var hash = _hasher.Hash(senha);

        //Act
        var resultado = _hasher.Verify("senhaErrada", hash);

        //Assert
        resultado.Should().BeFalse();
    }
}
