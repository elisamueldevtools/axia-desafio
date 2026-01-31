using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Auth;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Informe o login");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Informe a senha");
    }
}
