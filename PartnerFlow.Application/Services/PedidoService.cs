using PartnerFlow.Domain.Entities;
using PartnerFlow.Domain.Interfaces.Repositories;
using PartnerFlow.Domain.Interfaces.Services;

namespace PartnerFlow.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repository;

    public PedidoService(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task CriarPedidoAsync(Pedido pedido)
    {
        await _repository.CriarPedidoAsync(pedido);
    }

    public async Task<Pedido?> ObterPedidoAsync(Guid id)
    {
        return await _repository.ObterPedidoPorIdAsync(id);
    }
}