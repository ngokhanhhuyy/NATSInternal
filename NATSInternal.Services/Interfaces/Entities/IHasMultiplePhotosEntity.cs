namespace NATSInternal.Services.Interfaces.Entities;

internal interface IHasMultiplePhotosEntity<T, TPhoto> : IHasSinglePhotoEntity<T>
    where T : class, IUpsertableEntity<T>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
{
    List<TPhoto> Photos { get; set; }
}
