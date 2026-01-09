using KyleClough.EntityFrameworkExtensions.AuditLogging.DbContext;
using KyleClough.EntityFrameworkExtensions.AuditLogging.Entities;
using KyleClough.EntityFrameworkExtensions.AuditLogging.Extensions;
using Microsoft.EntityFrameworkCore;

namespace KyleClough.EntityFrameworkExtensions.AuditLoggingTests;

public class AuditLoggingTests
{
    // Simple entity to test auditing
    private class TestEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Local DbContext for testing
    private class TestDbContext : AuditDbContext
    {
        public TestDbContext(DbContextOptions<AuditDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    }

    [Fact]
    public async Task SaveChangesWithAuditAsync_Captures_Before_And_After_Values()
    {
        // Arrange: configure InMemory provider
        var options = new DbContextOptionsBuilder<AuditDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new TestDbContext(options);

        var entity = new TestEntity { Name = "Kyle" };
        context.TestEntities.Add(entity);
        await context.SaveChangesAsync();

        // Act: update entity
        entity.Name = "Kyle Updated";

        await context.SaveChangesWithAuditAsync(
            subjectId: "user-123",
            actor: "admin",
            correlationId: "corr-001",
            1,
            ipAddress: "127.0.0.1"
        );

        // Assert: verify audit log captured before/after
        var audit = context.AuditLogs.FirstOrDefault(a => a.PropertyName == "Name");
        Assert.NotNull(audit);
        Assert.Equal("Kyle", audit!.BeforeValue);
        Assert.Equal("Kyle Updated", audit.AfterValue);
        Assert.Equal("Modified", audit.Action);
    }
    
    [Fact]
    public async Task SaveChangesWithAuditAsync_Captures_BeforeValue_On_Delete()
    {
        var options = new DbContextOptionsBuilder<AuditDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new TestDbContext(options);

        var entity = new TestEntity { Name = "ToDelete" };
        context.TestEntities.Add(entity);
        await context.SaveChangesAsync();

        context.TestEntities.Remove(entity);

        await context.SaveChangesWithAuditAsync("user-123", "admin", "corr-del", 1);

        var audit = context.AuditLogs.FirstOrDefault(a => a.PropertyName == "Name");
        Assert.NotNull(audit);
        Assert.Equal("ToDelete", audit!.BeforeValue);
        Assert.Null(audit.AfterValue);
        Assert.Equal("Deleted", audit.Action);
    }
}