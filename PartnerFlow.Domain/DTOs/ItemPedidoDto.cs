namespace PartnerFlow.Domain.DTOs;

public class ItemPedidoDto
{
    public Guid Id { get; set; }
    public string? Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}