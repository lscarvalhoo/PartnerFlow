using PartnerFlow.Domain.Enums;

namespace PartnerFlow.Domain.Entities;

public class Pedido
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public DateTime Data { get; set; }
    public StatusPedido Status { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
}