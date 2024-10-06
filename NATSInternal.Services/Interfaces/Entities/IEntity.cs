namespace NATSInternal.Services.Interfaces.Entities;

internal interface IEntity<T> where T : class, new()
{
    static abstract void ConfigureModel(EntityTypeBuilder<T> entityBuilder);
}