using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using PartnerFlow.Application.Services;
using PartnerFlow.Domain.DTOs;
using PartnerFlow.Domain.Entities;
using PartnerFlow.Domain.Interfaces.Broker;
using PartnerFlow.Domain.Interfaces.Cache;
using PartnerFlow.Domain.Interfaces.Repositories;

namespace PartnerFlow.Tests.Application.Service;

public class PedidoServiceTests
{
    private readonly IPedidoRepository _pedidoRepository = Substitute.For<IPedidoRepository>();
    private readonly IItemPedidoRepository _itemRepository = Substitute.For<IItemPedidoRepository>();
    private readonly IKafkaProducer _kafkaProducer = Substitute.For<IKafkaProducer>();
    private readonly ICacheService _cacheService = Substitute.For<ICacheService>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IValidator<PedidoDto> _pedidoValidator = Substitute.For<IValidator<PedidoDto>>();

    private readonly PedidoService _service;

    public PedidoServiceTests()
    {
        _service = new PedidoService(
            _pedidoRepository,
            _itemRepository,
            _kafkaProducer,
            _cacheService,
            _mapper,
            _pedidoValidator
        );
    }

    [Fact]
    public async Task CriarPedidoAsync_DeveChamarRepositoriosEProducer()
    {
        var pedidoDto = new PedidoDto
        {
            ClienteId = Guid.NewGuid(),
            Itens = new List<ItemPedidoDto>
            {
                new() { Produto = "Produto 1", Quantidade = 2, PrecoUnitario = 10 }
            }
        };

        var pedido = new Pedido
        {
            ClienteId = pedidoDto.ClienteId,
            Itens = new List<ItemPedido>
            {
                new() { Produto = "Produto 1", Quantidade = 2, PrecoUnitario = 10 }
            }
        };

        _pedidoValidator.ValidateAsync(pedidoDto).Returns(new ValidationResult());
        _mapper.Map<Pedido>(pedidoDto).Returns(pedido);

        await _service.CriarPedidoAsync(pedidoDto);

        await _pedidoRepository.Received(1).CriarPedidoAsync(Arg.Any<Pedido>());
        await _itemRepository.Received(1).InserirItensAsync(Arg.Any<Guid>(), Arg.Any<List<ItemPedido>>());
        await _kafkaProducer.Received(1).PublishPedidoCriadoAsync(Arg.Any<Pedido>());
    }

    [Fact]
    public async Task CriarPedidoAsync_DeveLancarExcecao_SeValidacaoFalhar()
    {
        var pedidoDto = new PedidoDto();
        var validationFailures = new List<ValidationFailure>
        {
            new("ClienteId", "ClienteId é obrigatório")
        };
        _pedidoValidator.ValidateAsync(pedidoDto)
            .Returns(new ValidationResult(validationFailures));

        await Assert.ThrowsAsync<ValidationException>(() => _service.CriarPedidoAsync(pedidoDto));
    }

    [Fact]
    public async Task ObterPedidoAsync_DeveRetornarDoCache_SeExistir()
    {
        var id = Guid.NewGuid();
        var pedido = new Pedido { Id = id };
        _cacheService.GetAsync<Pedido>($"pedido:{id}").Returns(pedido);

        var result = await _service.ObterPedidoAsync(id);

        Assert.Equal(pedido, result);
        await _pedidoRepository.DidNotReceive().ObterPedidoPorIdAsync(id);
    }

    [Fact]
    public async Task ObterPedidoAsync_DeveBuscarEDefinirCache_SeNaoEstiverEmCache()
    {
        var id = Guid.NewGuid();
        var pedido = new Pedido { Id = id };
        var itens = new List<ItemPedido>
        {
            new() { Produto = "Produto X", Quantidade = 1, PrecoUnitario = 50 }
        };

        _cacheService.GetAsync<Pedido>($"pedido:{id}").Returns((Pedido?)null);
        _pedidoRepository.ObterPedidoPorIdAsync(id).Returns(pedido);
        _itemRepository.ObterItensPorPedidoId(id).Returns(itens);

        var result = await _service.ObterPedidoAsync(id);

        Assert.NotNull(result);
        Assert.Equal(pedido.Id, result?.Id);
        await _cacheService.Received(1).SetAsync($"pedido:{id}", Arg.Any<Pedido>(), TimeSpan.FromMinutes(2));
    }
}
