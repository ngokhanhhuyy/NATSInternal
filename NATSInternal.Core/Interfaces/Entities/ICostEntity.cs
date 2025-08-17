namespace NATSInternal.Core.Interfaces.Entities;

internal interface ICostEntity<T, TUpdateHistory, TData> : IHasStatsEntity<T, TUpdateHistory, TData>
    where T : class, IHasStatsEntity<T, TUpdateHistory, TData>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TData>
    where TData : class
{
    #region Properties
    long Amount { get; set; }
    #endregion
}
