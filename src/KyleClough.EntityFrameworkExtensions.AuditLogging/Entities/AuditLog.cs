namespace KyleClough.EntityFrameworkExtensions.AuditLogging.Entities;

/// <summary>
/// Represents a single audit trail entry recording changes to an entity,
/// including explicit before/after values.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Primary key identifier for the audit log entry.
    /// </summary>
    public int Id { get; set; }

    // Actor metadata

    /// <summary>
    /// Stable unique identifier of the subject (e.g., user ID or external identity claim).
    /// </summary>
    public string SubjectId { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable name of the actor who performed the action
    /// (e.g., username, system name, or service account).
    /// </summary>
    public string Actor { get; set; } = string.Empty;

    /// <summary>
    /// The type of action performed on the entity (Added, Modified, Deleted).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the action occurred.
    /// </summary>
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Correlation identifier used to group multiple changes within the same request or transaction.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// IP address of the client or system that initiated the change.
    /// </summary>
    public string? IpAddress { get; set; }

    // Entity metadata

    /// <summary>
    /// The name of the entity type affected (e.g., User, Order).
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// The primary key value of the entity instance affected.
    /// </summary>
    public string EntityId { get; set; } = string.Empty;

    // Change details

    /// <summary>
    /// The name of the property that was changed.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Value before the change (null for Added).
    /// </summary>
    public string? BeforeValue { get; set; }

    /// <summary>
    /// Value after the change (null for Deleted).
    /// </summary>
    public string? AfterValue { get; set; }
}