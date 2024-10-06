namespace NATSInternal.Services.Extensions;

internal static class EntityFrameworkExtensions
{
    internal static ModelBuilder ConfigureEntity<TEntity>(this ModelBuilder modelBuilder)
        where TEntity : class, IEntity<TEntity>
    {
        return modelBuilder.Entity<TEntity>(TEntity.ConfigureModel);
    }
}
