namespace PartnerFlow.Domain.DTOs;

public class PedidoDto
{
    public Guid ClienteId { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = new();
}