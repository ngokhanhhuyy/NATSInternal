namespace NATSInternal.Core.Extensions;

internal static class EntityFrameworkExtensions
{
    internal static ModelBuilder ConfigureEntity<T>(this ModelBuilder modelBuilder)
        where T : class, IEntity<T>, new()
    {
        return modelBuilder.Entity<T>(T.ConfigureModel);
    }
}
