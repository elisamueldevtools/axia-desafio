using Axia.Veiculos.Domain.Enums;
using FluentValidation;

namespace Axia.Veiculos.Application.UseCase.Veiculos.Commands;

public class UpdateVeiculoCommandValidator : AbstractValidator<UpdateVeiculoCommand>
{
    public UpdateVeiculoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id do veículo não informado");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(100).WithMessage("Descrição: máximo de 100 caracteres");

        RuleFor(x => x.Marca)
            .NotEmpty().WithMessage("Informe a marca")
            .Must(BeValidMarca).WithMessage("Selecione uma marca válida");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("Informe o modelo")
            .MaximumLength(30).WithMessage("Modelo não pode ter mais de 30 caracteres");

        When(x => x.Valor.HasValue, () =>
        {
            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor precisa ser positivo");
        });
    }

    private static bool BeValidMarca(string marca)
    {
        return Enum.TryParse<Marca>(marca, ignoreCase: true, out _);
    }
}
