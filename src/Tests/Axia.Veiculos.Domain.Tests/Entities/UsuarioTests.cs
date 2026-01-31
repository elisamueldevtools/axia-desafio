using Axia.Veiculos.Domain.Entities;
using Axia.Veiculos.Domain.Enums;
using Xunit;

namespace Axia.Veiculos.Domain.Tests.Entities;

public class UsuarioTests
{
    [Fact]
    public void Create_DeveRetornarUsuarioComDadosCorretos()
    {
        //Arrange
        var nome = "João Silva";
        var login = "joao.silva";
        var hash = "$2a$11$hash";

        //Act
        var usuario = Usuario.Create(nome, login, hash, Role.User);

        //Assert
        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(login, usuario.Login);
        Assert.Equal(hash, usuario.SenhaHash);
        Assert.Equal(Role.User, usuario.Role);
    }

    [Fact]
    public void Update_DeveAtualizarNomeERole()
    {
        //Arrange
        var usuario = Usuario.Create("Nome Antigo", "login", "hash", Role.Reader);

        //Act
        usuario.Update("Nome Novo", Role.Admin);

        //Assert
        Assert.Equal("Nome Novo", usuario.Nome);
        Assert.Equal(Role.Admin, usuario.Role);
        Assert.Equal("login", usuario.Login); // login nao muda
    }

    [Fact]
    public void UpdateSenha_DeveAtualizarHash()
    {
        //Arrange
        var usuario = Usuario.Create("Nome", "login", "senha.antiga");

        //Act
        usuario.UpdateSenha("senha.nova");

        //Assert
        Assert.Equal("senha.nova", usuario.SenhaHash);
    }
}
