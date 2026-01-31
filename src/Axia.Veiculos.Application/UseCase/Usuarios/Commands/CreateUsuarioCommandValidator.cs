using Axia.Veiculos.Domain.Enums;
using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Informe o login")
            .MinimumLength(3).WithMessage("Login deve conter no mínimo 3 caracteres");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha obrigatória")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Informe a role")
            .Must(BeValidRole).WithMessage("Role inválida. Valores permitidos: Reader, User, Admin");
    }

    private static bool BeValidRole(string role)
    {
        return Enum.TryParse<Role>(role, ignoreCase: true, out _);
    }
}
