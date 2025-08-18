using MongoDB.Bson.Serialization.Attributes;
using PartnerFlow.Domain.Enums;

namespace PartnerFlow.Domain.Entities;

public class ItemPedido
{
    [BsonId]
    public Guid Id { get; set; }

    public Guid PedidoId { get; set; }

    public string? Produto { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public StatusPedido Status { get; set; }

    public DateTime DataCriacao { get; set; }
}
