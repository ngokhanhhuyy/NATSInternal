namespace NATSInternal.Services.Interfaces.Entities;

internal interface IHasPhotoEntity<T> : IUpsertableEntity<T>
    where T : class, IUpsertableEntity<T>, new();
