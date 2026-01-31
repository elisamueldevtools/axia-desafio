namespace Axia.Veiculos.Application.UseCase.Usuarios.Requests;

public record CreateUsuarioRequest(string Nome, string Login, string Senha, string Role = "Reader");
