namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpsertableEntity<T> : IHasIdEntity<T> where T : class
{
    #region Properties
    DateTime CreatedDateTime { get; }
    #endregion
}