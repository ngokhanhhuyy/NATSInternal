namespace NATSInternal.Services.Interfaces.Entities;

internal interface IHasPhotoEntity<T> : IUpsertableEntity<T>
    where T : class, IUpsertableEntity<T>, new()
{
    string ThumbnailUrl { get; }
}

internal interface IHasPhotoEntity<T, TPhoto> : IHasPhotoEntity<T>
    where T : class, IUpsertableEntity<T>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
{
    List<TPhoto> Photos { get; set; }
}
