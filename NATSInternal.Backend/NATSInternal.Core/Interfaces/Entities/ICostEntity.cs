namespace NATSInternal.Core.Interfaces.Entities;

internal interface ICostEntity<T, TData> : IHasStatsEntity<T, TData>
    where T : class, IHasStatsEntity<T, TData>
    where TData : class
{
    #region Properties
    long Amount { get; set; }
    #endregion
}
