namespace NATSInternal.Core.Interfaces.Entities;

internal interface IPhotoEntity<T> : IEntity<T> where T : class, new()
{
    string Url { get; set; }
}
