using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KyleClough.EntityFrameworkExtensions.AuditLogging.DbContext;

/// <summary>
/// Design-time factory for creating AuditDbContext instances with PostgresSQL.
/// Required by EF Core tools to scaffold and apply migrations inside a class library.
/// </summary>
public class AuditDbContextFactory : IDesignTimeDbContextFactory<AuditDbContext>
{
    public AuditDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuditDbContext>();

        // PostgresSQL provider
        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=AuditDb;Username=postgres;Password=yourpassword");

        return new AuditDbContext(optionsBuilder.Options);
    }
}