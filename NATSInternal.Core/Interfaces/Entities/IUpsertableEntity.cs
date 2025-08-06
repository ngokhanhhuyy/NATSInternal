namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpsertableEntity<T> : IIdentifiableEntity<T> where T : class, new()
{
    DateTime CreatedDateTime { get; set; }
}