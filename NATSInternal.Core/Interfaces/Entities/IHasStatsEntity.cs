namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasStatsEntity<T, TUpdateHistory>
    :
        ICreatorTrackableEntity<T>,
        IUpdaterTrackableEntity<T, TUpdateHistory>
    where T : class, IUpsertableEntity<T>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>
{
    DateTime StatsDateTime { get; set; }
    string? Note { get; set; }
    bool IsDeleted { get; set; }
    bool IsLocked { get; }
}