namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICostEntity<T, TUpdateHistory>
    : IHasStatsEntity<T, TUpdateHistory>
    where T : class, IHasStatsEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    long Amount { get; set; }
}
