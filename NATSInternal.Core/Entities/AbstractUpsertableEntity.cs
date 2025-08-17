namespace NATSInternal.Core.Entities;

internal abstract class AbstractUpsertableEntity<T> : AbstractHasIdEntity<T>, IUpsertableEntity<T>
    where T : AbstractUpsertableEntity<T>
{
    #region Properties
    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();
    #endregion
}