namespace PartnerFlow.Domain.Entities;

public class ItemPedido
{
    public Guid PedidoId { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }

    public decimal Subtotal => Quantidade * PrecoUnitario;
}
