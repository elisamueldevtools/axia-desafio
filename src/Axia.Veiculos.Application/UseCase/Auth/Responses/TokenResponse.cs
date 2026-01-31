namespace Axia.Veiculos.Application.UseCase.Auth.Responses;

public record TokenResponse(string AccessToken, string TokenType = "Bearer");
