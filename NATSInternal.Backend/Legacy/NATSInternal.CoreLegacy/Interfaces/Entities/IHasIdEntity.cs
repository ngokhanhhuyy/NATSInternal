namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasIdEntity<TEntity> : IEntity<TEntity> where TEntity : class
{
    #region Properties
    Guid Id { get; }
    #endregion
}