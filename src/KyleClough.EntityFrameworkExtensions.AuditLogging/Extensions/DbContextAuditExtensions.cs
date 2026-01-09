using KyleClough.EntityFrameworkExtensions.AuditLogging.Entities;
using Microsoft.EntityFrameworkCore;
using KyleClough.EntityFrameworkExtensions.AuditLogging.DbContext;

namespace KyleClough.EntityFrameworkExtensions.AuditLogging.Extensions;

/// <summary>
/// Extension methods for DbContext to enable automatic audit logging.
/// </summary>
public static class DbContextAuditExtensions
{
    /// <summary>
    /// Saves changes to the database and automatically writes audit logs
    /// for entities that inherit from <see cref="AuditableEntity"/>.
    /// </summary>
    /// <param name="context">The DbContext instance.</param>
    /// <param name="subjectId">Stable identifier of the actor (e.g., user ID).</param>
    /// <param name="actor">Display name of the actor.</param>
    /// <param name="correlationId">Correlation ID for grouping logs in a single request.</param>
    /// <param name="userId">User ID making the request.</param>
    /// <param name="ipAddress">Optional IP address of the client.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public static async Task<int> SaveChangesWithAuditAsync(
        this AuditDbContext context,
        string subjectId,
        string actor,
        string correlationId,
        int userId,
        string? ipAddress = null)
    {
        var auditEntries = new List<AuditLog>();
        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added ||
                entry.State == EntityState.Modified ||
                entry.State == EntityState.Deleted)
            {
                foreach (var prop in entry.Properties)
                {
                    // Only log modified properties for updates
                    if (entry.State == EntityState.Modified && !prop.IsModified)
                        continue;

                    var audit = new AuditLog
                    {
                        SubjectId = subjectId,
                        Actor = actor,
                        Action = entry.State.ToString(),
                        EntityName = entry.Entity.GetType().Name,
                        EntityId = entry.Property("Id").CurrentValue?.ToString() ?? string.Empty,
                        PropertyName = prop.Metadata.Name,
                        TimestampUtc = utcNow,
                        CorrelationId = correlationId,
                        IpAddress = ipAddress,
                        BeforeValue = entry.State == EntityState.Added ? null : prop.OriginalValue?.ToString(),
                        AfterValue = entry.State == EntityState.Deleted ? null : prop.CurrentValue?.ToString()
                    };

                    auditEntries.Add(audit);
                }

                // Update audit metadata on the entity itself
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.CreatedBy = userId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                    entry.Entity.UpdatedBy = userId;
                }
            }
        }

        // Save audit logs into the AuditDbContext
        if (auditEntries.Any())
        {
            context.Set<AuditLog>().AddRange(auditEntries);
        }

        return await context.SaveChangesAsync();
    }
}