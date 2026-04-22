namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasStatsEntity<T, TData> : ICreatorTrackableEntity<T>, IUpdaterTrackableEntity<T, TData>
    where T : class, IUpsertableEntity<T>
    where TData : class
{
    #region Properties
    DateTime StatsDateTime { get; set; }
    string? Note { get; set; }
    bool IsDeleted { get; set; }
    #endregion
}