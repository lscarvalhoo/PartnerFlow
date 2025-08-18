using MongoDB.Driver;
using PartnerFlow.Domain.Entities;
using PartnerFlow.Domain.Interfaces.Repositories;

namespace PartnerFlow.Infrastructure.Persistence.Mongo;

public class ItemPedidoMongoRepository : IItemPedidoRepository
{
    private readonly MongoContext _context;

    public ItemPedidoMongoRepository(MongoContext context)
    {
        _context = context;
    }

    public async Task InserirItensAsync(Guid pedidoId, List<ItemPedido> itens)
    {
        foreach (var item in itens)
        {
            item.PedidoId = pedidoId;
            item.DataCriacao = DateTime.Now;
            item.Status = Domain.Enums.StatusPedido.Criado;
        }
        await _context.ItensPedido.InsertManyAsync(itens);
    }

    public async Task<List<ItemPedido>> ObterItensPorPedidoId(Guid pedidoId)
    {
        return await _context.ItensPedido.Find(x => x.PedidoId == pedidoId).ToListAsync();
    }
}
