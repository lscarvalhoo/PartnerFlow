using Microsoft.EntityFrameworkCore;
using PartnerFlow.Domain.Entities;
using PartnerFlow.Domain.Interfaces.Repositories;

namespace PartnerFlow.Infrastructure.Persistence.Sql;

public class PedidoSqlRepository : IPedidoRepository
{
    private readonly PartnerFlowDbContext _context;

    public PedidoSqlRepository(PartnerFlowDbContext context)
    {
        _context = context;
    }

    public async Task CriarPedidoAsync(Pedido pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task<Pedido?> ObterPedidoPorIdAsync(Guid id)
    {
        return await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
    }
}