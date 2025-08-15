using PartnerFlow.Domain.Entities;

namespace PartnerFlow.Domain.Interfaces.Services;

public interface IPedidoService
{
    Task CriarPedidoAsync(Pedido pedido);
    Task<Pedido?> ObterPedidoAsync(Guid id);
}