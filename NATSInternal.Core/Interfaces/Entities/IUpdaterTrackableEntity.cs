namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpdaterTrackableEntity<T, TUpdateHistory> : IUpsertableEntity<T>
    where T : class, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    List<TUpdateHistory> UpdateHistories { get; set; }
}