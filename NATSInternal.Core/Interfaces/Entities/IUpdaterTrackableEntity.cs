namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpdaterTrackableEntity<T, TUpdateHistory, TData> : IUpsertableEntity<T>
    where T : class
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TData>
    where TData : class
{
    List<TUpdateHistory> UpdateHistories { get; }
}