using PartnerFlow.Domain.DTOs;
using PartnerFlow.Domain.Entities;

namespace PartnerFlow.Domain.Interfaces.Services;

public interface IPedidoService
{
    Task CriarPedidoAsync(PedidoDto pedido);
    Task<Pedido?> ObterPedidoAsync(Guid id);
}