namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpdaterTrackableEntity<T, TData> : IUpsertableEntity<T>
    where T : class
    where TData : class
{
    List<UpdateHistory> UpdateHistories { get; }
}