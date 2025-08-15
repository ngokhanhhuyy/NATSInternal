namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpdaterTrackableEntity<T, TUpdateHistory> : IUpsertableEntity<T>
    where T : class
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>
{
    List<TUpdateHistory> UpdateHistories { get; }
}