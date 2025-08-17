namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasStatsEntity<T, TUpdateHistory, TData>
    :
        ICreatorTrackableEntity<T>,
        IUpdaterTrackableEntity<T, TUpdateHistory, TData>
    where T : class, IUpsertableEntity<T>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TData>
    where TData : class
{
    #region Properties
    DateTime StatsDateTime { get; set; }
    string? Note { get; set; }
    bool IsDeleted { get; set; }
    bool IsLocked { get; }
    #endregion
}