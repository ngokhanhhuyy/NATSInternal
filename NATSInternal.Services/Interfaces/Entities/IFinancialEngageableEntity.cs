namespace NATSInternal.Services.Interfaces.Entities;

internal interface IFinancialEngageableEntity<T, TUser, TUpdateHistory>
    :
        ICreatorTrackableEntity<T, TUser>,
        IUpdaterTrackableEntity<T, TUser, TUpdateHistory>
    where T : class, IUpsertableEntity<T>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    DateTime StatsDateTime { get; set; }
    string Note { get; set; }
    bool IsLocked { get; }
}