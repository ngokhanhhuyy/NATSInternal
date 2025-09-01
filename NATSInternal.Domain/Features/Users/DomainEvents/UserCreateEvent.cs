using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserCreateEvent : IDomainEvent
{
    #region Constructors
    internal UserCreateEvent(Guid createdUserId, DateTime createdDateTime)
    {
        CreatedUserId = createdUserId;
        CreatedDateTime = createdDateTime;
    }
    #endregion
    
    #region Properties
    public Guid CreatedUserId { get; }
    public DateTime CreatedDateTime { get; }
    #endregion
}