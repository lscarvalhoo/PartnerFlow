using PartnerFlow.Domain.Entities;
using PartnerFlow.Domain.Interfaces.Broker;
using PartnerFlow.Domain.Interfaces.Repositories;
using PartnerFlow.Domain.Interfaces.Services;

namespace PartnerFlow.Application.Services;


public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IItemPedidoRepository _itemRepository;
    private readonly IKafkaProducer _kafkaProducer;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IItemPedidoRepository itemRepository,
        IKafkaProducer kafkaProducer)
    {
        _pedidoRepository = pedidoRepository;
        _itemRepository = itemRepository;
        _kafkaProducer = kafkaProducer;
    }

    public async Task CriarPedidoAsync(Pedido pedido)
    {
        await _pedidoRepository.CriarPedidoAsync(pedido);
        await _itemRepository.InserirItensAsync(pedido.Id, pedido.Itens);
        await _kafkaProducer.PublishPedidoCriadoAsync(pedido);
    }

    public async Task<Pedido?> ObterPedidoAsync(Guid id)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorIdAsync(id);
        if (pedido == null) 
            return null;

        pedido.Itens = await _itemRepository.ObterItensPorPedidoId(id);
        return pedido;
    }
}