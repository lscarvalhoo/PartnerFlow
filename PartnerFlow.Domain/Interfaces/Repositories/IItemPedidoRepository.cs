using PartnerFlow.Domain.DTOs;
using PartnerFlow.Domain.Entities;

namespace PartnerFlow.Domain.Interfaces.Repositories
{
    public interface IItemPedidoRepository
    {
        Task InserirItensAsync(Guid pedidoId, List<ItemPedido> itens);
        Task<List<ItemPedido>> ObterItensPorPedidoId(Guid pedidoId);
    }
}
