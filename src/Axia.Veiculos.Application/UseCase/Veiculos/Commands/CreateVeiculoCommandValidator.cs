using Axia.Veiculos.Domain.Enums;
using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public class CreateVeiculoCommandValidator : AbstractValidator<CreateVeiculoCommand>
{
    public CreateVeiculoCommandValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Informe a descrição do veículo")
            .MaximumLength(100).WithMessage("Descrição não pode ultrapassar 100 caracteres");

        RuleFor(x => x.Marca)
            .NotEmpty().WithMessage("Informe a marca")
            .Must(BeValidMarca).WithMessage("Marca inválida");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("Modelo é obrigatório")
            .MaximumLength(30).WithMessage("Modelo: máximo 30 caracteres");

        When(x => x.Valor.HasValue, () =>
        {
            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("Valor deve ser maior que zero");
        });
    }

    private static bool BeValidMarca(string marca)
    {
        return Enum.TryParse<Marca>(marca, ignoreCase: true, out _);
    }
}
