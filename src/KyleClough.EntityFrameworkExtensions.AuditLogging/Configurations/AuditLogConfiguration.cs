using KyleClough.EntityFrameworkExtensions.AuditLogging.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KyleClough.EntityFrameworkExtensions.AuditLogging.Configurations;

/// <summary>
/// EF Core configuration for the AuditLog entity.
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.SubjectId)
            .HasMaxLength(128)
            .IsRequired();

        entity.Property(e => e.Actor)
            .HasMaxLength(256)
            .IsRequired();

        entity.Property(e => e.Action)
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(e => e.EntityName)
            .HasMaxLength(256)
            .IsRequired();

        entity.Property(e => e.EntityId)
            .HasMaxLength(128);

        entity.Property(e => e.PropertyName)
            .HasMaxLength(256);

        entity.Property(e => e.BeforeValue)
            .HasColumnType("text");

        entity.Property(e => e.AfterValue)
            .HasColumnType("text");

        entity.Property(e => e.TimestampUtc)
            .IsRequired();

        entity.Property(e => e.CorrelationId)
            .HasMaxLength(128);

        entity.Property(e => e.IpAddress)
            .HasMaxLength(64);
    }
}