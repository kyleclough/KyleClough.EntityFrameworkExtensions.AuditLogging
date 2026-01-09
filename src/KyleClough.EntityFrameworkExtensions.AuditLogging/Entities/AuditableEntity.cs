namespace KyleClough.EntityFrameworkExtensions.AuditLogging.Entities;

/// <summary>
/// Base class for entities that require audit metadata.
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// UTC timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Identifier of the actor who created the entity.
    /// </summary>
    public int CreatedBy { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identifier of the actor who last updated the entity.
    /// </summary>
    public int? UpdatedBy { get; set; }
}
