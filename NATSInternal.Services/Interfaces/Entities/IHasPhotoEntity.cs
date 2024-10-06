namespace NATSInternal.Services.Interfaces.Entities;

internal interface IHasPhotoEntity<T, TPhoto> : IUpsertableEntity<T>
    where T : class, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
{
    List<TPhoto> Photos { get; set; }
}
