namespace NATSInternal.Core.Interfaces.Entities;

internal interface IPhotoEntity<T> : IHasIdEntity<T> where T : class
{
    #region Properties
    string Url { get; set; }
    #endregion
}