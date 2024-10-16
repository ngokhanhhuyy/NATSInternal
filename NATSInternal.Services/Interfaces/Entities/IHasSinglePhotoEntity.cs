namespace NATSInternal.Services.Interfaces.Entities;

internal interface IHasSinglePhotoEntity<T> : IUpsertableEntity<T>
    where T : class, IUpsertableEntity<T>, new()
{
    string ThumbnailUrl { get; }
}