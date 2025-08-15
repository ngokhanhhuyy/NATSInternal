namespace NATSInternal.Core.Interfaces.Entities;

internal interface IUpdateHistoryEntity<T> : IHasIdEntity<T> where T : class
{
    DateTime UpdatedDateTime { get; set; }
    string Reason { get; set; }
    string OldData { get; set; }
    string NewData { get; set; }
    Guid UpdatedUserId { get; set; }
    User UpdatedUser { get; set; }
}