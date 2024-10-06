namespace NATSInternal.Services.Interfaces.Entities;

internal interface IUpdaterTrackableEntity<T, TUser, TUpdateHistory> : IUpsertableEntity<T>
    where T : class, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    List<TUpdateHistory> UpdateHistories { get; set; }
}