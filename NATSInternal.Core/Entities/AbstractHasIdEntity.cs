namespace NATSInternal.Core.Entities;

internal abstract class AbstractHasIdEntity<T> : AbstractEntity<T> where T : AbstractHasIdEntity<T>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();
    #endregion

    #region StaticMethods
    protected static void ConfigureModel(EntityTypeBuilder<T> entityBuilder)
    {
        entityBuilder.HasKey(e => e.Id);
    }
    #endregion
}