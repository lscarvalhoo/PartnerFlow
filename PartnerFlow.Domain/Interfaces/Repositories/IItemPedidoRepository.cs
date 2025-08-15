using PartnerFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerFlow.Domain.Interfaces.Repositories
{
    public interface IItemPedidoRepository
    {
        Task InserirItensAsync(Guid pedidoId, List<ItemPedido> itens);
        Task<List<ItemPedido>> ObterItensPorPedidoId(Guid pedidoId);
    }
}
