using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public class UpdateVeiculoCommandValidator : AbstractValidator<UpdateVeiculoCommand>
{
    public UpdateVeiculoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id obrigatório");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição obrigatória")
            .MaximumLength(100).WithMessage("Descrição: máx 100 caracteres");

        RuleFor(x => x.Marca).IsInEnum().WithMessage("Marca inválida");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("Modelo obrigatório")
            .MaximumLength(30).WithMessage("Modelo: máx 30 caracteres");

        When(x => x.Valor.HasValue, () =>
            RuleFor(x => x.Valor).GreaterThan(0).WithMessage("Valor deve ser positivo"));
    }
}
