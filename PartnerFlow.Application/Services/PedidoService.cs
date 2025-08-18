using AutoMapper;
using FluentValidation;
using PartnerFlow.Domain.DTOs;
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
    private readonly IMapper _mapper;
    private readonly IValidator<PedidoDto> _pedidoValidator;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IItemPedidoRepository itemRepository,
        IKafkaProducer kafkaProducer,
        ICacheService cacheService,
        IMapper mapper,
        IValidator<PedidoDto> pedidoValidator)
    {
        _pedidoRepository = pedidoRepository;
        _itemRepository = itemRepository;
        _kafkaProducer = kafkaProducer;
        _cacheService = cacheService;
        _mapper = mapper;
        _pedidoValidator = pedidoValidator;
    }

    public async Task CriarPedidoAsync(PedidoDto pedidoDto)
    {
        var validationResult = await _pedidoValidator.ValidateAsync(pedidoDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var pedido = _mapper.Map<Pedido>(pedidoDto);
        pedido.Id = Guid.NewGuid();

        foreach (var item in pedido.Itens)
            item.PedidoId = pedido.Id;

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