using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PartnerFlow.Infrastructure.Persistence.Sql;

public class PartnerFlowDbContextFactory : IDesignTimeDbContextFactory<PartnerFlowDbContext>
{
    public PartnerFlowDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PartnerFlowDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=PartnerFlowDb;User Id=sa;Password=StrongP@ssw0rd;TrustServerCertificate=True;");

        return new PartnerFlowDbContext(optionsBuilder.Options);
    }
}
