namespace NATSInternal.Core.Interfaces.Entities;

internal interface IEntity<TEntity> where TEntity : class
{
    static abstract void ConfigureModel(EntityTypeBuilder<TEntity> entityBuilder);
}