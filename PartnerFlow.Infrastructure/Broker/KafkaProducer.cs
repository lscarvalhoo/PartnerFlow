using Confluent.Kafka;
using Microsoft.Extensions.Options;
using PartnerFlow.Domain.Entities;
using PartnerFlow.Domain.Interfaces.Broker;
using PartnerFlow.Infrastructure.Config;
using System.Text.Json;

namespace PartnerFlow.Infrastructure.Broker;

public class KafkaProducer : IKafkaProducer
{
    private readonly KafkaSettings _settings;
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IOptions<KafkaSettings> options)
    {
        _settings = options.Value;

        var config = new ProducerConfig
        {
            BootstrapServers = _settings.Broker,
            Acks = Acks.All,
            RequestTimeoutMs = 5000,
            SocketTimeoutMs = 3000,
            EnableIdempotence = true
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishPedidoCriadoAsync(Pedido pedido)
    {
        try
        {
            foreach (var item in pedido.Itens)
            {
                var evento = new
                {
                    Evento = "ItemPedidoCriado",
                    PedidoId = pedido.Id,
                    ClienteId = pedido.ClienteId,
                    ItemId = item.Id,
                    Produto = item.Produto,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    Status = item.Status.ToString(),
                    DataCriacao = item.DataCriacao
                };

                var mensagem = JsonSerializer.Serialize(evento);

                await _producer.ProduceAsync(
                    _settings.Topic,
                    new Message<Null, string> { Value = mensagem }
                );
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao publicar mensagem no Kafka.", ex);
        }
    }

}