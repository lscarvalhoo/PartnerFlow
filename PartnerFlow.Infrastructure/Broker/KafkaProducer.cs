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
            BootstrapServers = _settings.Broker
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishPedidoCriadoAsync(Pedido pedido)
    {
        var evento = new
        {
            PedidoId = pedido.Id,
            ClienteId = pedido.ClienteId,
            Data = pedido.Data,
            Status = pedido.Status
        };

        var mensagem = JsonSerializer.Serialize(evento);
        await _producer.ProduceAsync(_settings.Topic, new Message<Null, string> { Value = mensagem });
    }
}
