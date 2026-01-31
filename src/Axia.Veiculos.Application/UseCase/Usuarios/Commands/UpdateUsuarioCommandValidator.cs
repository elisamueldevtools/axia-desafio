using Axia.Veiculos.Domain.Enums;
using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Usuarios.Commands;

public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
{
    public UpdateUsuarioCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id obrigatório");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome obrigatório")
            .MinimumLength(3).WithMessage("Nome: mín 3 caracteres");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role obrigatória")
            .Must(r => Enum.TryParse<Role>(r, true, out _)).WithMessage("Role inválida");
    }
}
