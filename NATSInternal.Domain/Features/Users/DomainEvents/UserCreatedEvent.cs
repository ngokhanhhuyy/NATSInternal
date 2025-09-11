using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserCreatedEvent : IDomainEvent
{
    #region Constructors
    internal UserCreatedEvent(Guid createdUserId, DateTime createdDateTime)
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