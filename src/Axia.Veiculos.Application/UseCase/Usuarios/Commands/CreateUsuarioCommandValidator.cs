using Axia.Veiculos.Domain.Enums;
using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome obrigatório")
            .MinimumLength(3).WithMessage("Nome: mín 3 caracteres");

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login obrigatório")
            .MinimumLength(3).WithMessage("Login: mín 3 caracteres");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha obrigatória")
            .MinimumLength(6).WithMessage("Senha: mín 6 caracteres");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role obrigatória")
            .Must(r => Enum.TryParse<Role>(r, true, out _)).WithMessage("Role inválida");
    }
}
