using Microsoft.EntityFrameworkCore;
using PartnerFlow.Domain.Entities;

namespace PartnerFlow.Infrastructure.Persistence.Sql;

public class PartnerFlowDbContext : DbContext
{
    public PartnerFlowDbContext(DbContextOptions<PartnerFlowDbContext> options)
        : base(options) { }

    public DbSet<Pedido> Pedidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.ClienteId).IsRequired();
            e.Ignore(p => p.Itens);
        });
    }
}