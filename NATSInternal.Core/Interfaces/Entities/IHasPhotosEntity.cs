namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasPhotosEntity<T> : IUpsertableEntity<T> where T : class
{
    #region Properties
    List<Photo> Photos { get; }
    #endregion
}