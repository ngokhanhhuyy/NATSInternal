namespace NATSInternal.Services.Interfaces.Entities;

internal interface IUpdateHistoryEntity<T, TUser> : IIdentifiableEntity<T>
    where T : class, new()
    where TUser : class, IUserEntity<TUser>, new()
{
    DateTime UpdatedDateTime { get; set; }
    string Reason { get; set; }
    string OldData { get; set; }
    string NewData { get; set; }
    int UpdatedUserId { get; set; }
    TUser UpdatedUser { get; set; }
}