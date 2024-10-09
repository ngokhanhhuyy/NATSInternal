namespace NATSInternal.Services.Interfaces.Entities;

internal interface IFinancialEngageableEntity<T, TUpdateHistory>
    :
        ICreatorTrackableEntity<T>,
        IUpdaterTrackableEntity<T, TUpdateHistory>
    where T : class, IUpsertableEntity<T>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    DateTime StatsDateTime { get; set; }
    string Note { get; set; }
    bool IsDeleted { get; set; }
    bool IsLocked { get; }

    abstract static Expression<Func<T, DateTime>> StatsDateTimeExpression { get; }
}