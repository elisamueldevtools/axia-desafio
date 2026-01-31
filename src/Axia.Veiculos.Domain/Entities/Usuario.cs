using Axia.Veiculos.Domain.Enums;

namespace Axia.Veiculos.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Login { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public Role Role { get; private set; }

    protected Usuario() { }

    public static Usuario Create(string nome, string login, string senhaHash, Role role = Role.Reader)
    {
        return new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Login = login,
            SenhaHash = senhaHash,
            Role = role
        };
    }

    public void Update(string nome, Role role)
    {
        Nome = nome;
        Role = role;
    }

    public void UpdateSenha(string senhaHash)
    {
        SenhaHash = senhaHash;
    }
}
