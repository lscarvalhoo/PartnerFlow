using FluentValidation;
using PartnerFlow.Domain.DTOs;

namespace PartnerFlow.Application.Validations;

public class ItemPedidoDtoValidator : AbstractValidator<ItemPedidoDto>
{
    public ItemPedidoDtoValidator()
    {
        RuleFor(i => i.Produto)
            .NotEmpty().WithMessage("Produto é obrigatório.")
            .MaximumLength(100).WithMessage("Produto deve ter no máximo 100 caracteres.");

        RuleFor(i => i.Quantidade)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");

        RuleFor(i => i.PrecoUnitario)
            .GreaterThan(0).WithMessage("Preço unitário deve ser maior que zero.");
    }
}
