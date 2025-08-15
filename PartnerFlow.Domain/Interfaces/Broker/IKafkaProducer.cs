using PartnerFlow.Domain.Entities;

namespace PartnerFlow.Domain.Interfaces.Broker;

public interface IKafkaProducer
{
    Task PublishPedidoCriadoAsync(Pedido pedido);
}