using Axia.Veiculos.Domain.Enums;
using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
{
    public UpdateUsuarioCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Informe o Id do usuário");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome não pode ser vazio")
            .MinimumLength(3).WithMessage("Nome precisa ter ao menos 3 caracteres");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Informe a role")
            .Must(BeValidRole).WithMessage("Role inválida. Valores aceitos: Reader, User, Admin");
    }

    private static bool BeValidRole(string role)
    {
        return Enum.TryParse<Role>(role, ignoreCase: true, out _);
    }
}
