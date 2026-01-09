using KyleClough.EntityFrameworkExtensions.AuditLogging.Entities;
using KyleClough.EntityFrameworkExtensions.AuditLogging.Configurations;
using Microsoft.EntityFrameworkCore;

namespace KyleClough.EntityFrameworkExtensions.AuditLogging.DbContext;

public class AuditDbContext(DbContextOptions<AuditDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configuration from separate file
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
    }
}