namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpdateHistoryEntity<TEntity, TData> : IHasIdEntity<TEntity>
    where TEntity : class
    where TData : class
{
    #region Properties
    DateTime UpdatedDateTime { get; set; }
    string Reason { get; set; }
    TData OldData { get; set; }
    TData NewData { get; set; }
    Guid UpdatedUserId { get; set; }
    User UpdatedUser { get; set; }
    #endregion
}