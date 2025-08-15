namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpsertableEntity<T> : IHasIdEntity<T> where T : class
{
    DateTime CreatedDateTime { get; set; }
}