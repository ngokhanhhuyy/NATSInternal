namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasThumbnailEntity<T> : IHasPhotosEntity<T> where T : class, IUpsertableEntity<T>
{
    #region
    string? ThumbnailUrl { get; }
    #endregion
}