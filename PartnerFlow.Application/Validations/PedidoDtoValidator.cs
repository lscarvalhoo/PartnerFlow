using FluentValidation;
using PartnerFlow.Domain.DTOs;

namespace PartnerFlow.Application.Validations;

public class PedidoDtoValidator : AbstractValidator<PedidoDto>
{
    public PedidoDtoValidator()
    {
        RuleFor(p => p.ClienteId)
            .NotEmpty().WithMessage("ClienteId é obrigatório.");

        RuleFor(p => p.Itens)
            .NotNull().WithMessage("Itens não pode ser nulo.")
            .Must(itens => itens.Any()).WithMessage("É necessário ao menos um item no pedido.");

        RuleForEach(p => p.Itens).SetValidator(new ItemPedidoDtoValidator());
    }
}

