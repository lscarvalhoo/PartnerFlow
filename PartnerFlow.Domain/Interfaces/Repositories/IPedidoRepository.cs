using PartnerFlow.Domain.Entities;

namespace PartnerFlow.Domain.Interfaces.Repositories;

public interface IPedidoRepository
{
    Task CriarPedidoAsync(Pedido pedido);
    Task<Pedido?> ObterPedidoPorIdAsync(Guid id);
}