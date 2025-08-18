using PartnerFlow.Domain.DTOs;

namespace PartnerFlow.Domain.Entities;

public class Pedido
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
}