namespace NATSInternal.Services.Interfaces.Entities;

internal interface IUpdateHistoryEntity<T> : IIdentifiableEntity<T>
    where T : class, new()
{
    DateTime UpdatedDateTime { get; set; }
    string Reason { get; set; }
    string OldData { get; set; }
    string NewData { get; set; }
    int UpdatedUserId { get; set; }
    User UpdatedUser { get; set; }
}