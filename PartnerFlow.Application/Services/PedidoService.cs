using PartnerFlow.Domain.Entities;
using PartnerFlow.Domain.Interfaces.Broker;
using PartnerFlow.Domain.Interfaces.Cache;
using PartnerFlow.Domain.Interfaces.Repositories;
using PartnerFlow.Domain.Interfaces.Services;

namespace PartnerFlow.Application.Services;


public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IItemPedidoRepository _itemRepository;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly ICacheService _cacheService;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IItemPedidoRepository itemRepository,
        IKafkaProducer kafkaProducer,
        ICacheService cacheService)
    {
        _pedidoRepository = pedidoRepository;
        _itemRepository = itemRepository;
        _kafkaProducer = kafkaProducer;
        _cacheService = cacheService;
    }

    public async Task CriarPedidoAsync(Pedido pedido)
    {
        await _pedidoRepository.CriarPedidoAsync(pedido);
        await _itemRepository.InserirItensAsync(pedido.Id, pedido.Itens);
        await _kafkaProducer.PublishPedidoCriadoAsync(pedido);
    }

    public async Task<Pedido?> ObterPedidoAsync(Guid id)
    {
        var cacheKey = $"pedido:{id}";
        var pedido = await _cacheService.GetAsync<Pedido>(cacheKey);

        if (pedido != null)
            return pedido;

        pedido = await _pedidoRepository.ObterPedidoPorIdAsync(id);
        if (pedido == null)
            return null;

        pedido.Itens = await _itemRepository.ObterItensPorPedidoId(id);

        await _cacheService.SetAsync(cacheKey, pedido, TimeSpan.FromMinutes(2));

        return pedido;
    }
}