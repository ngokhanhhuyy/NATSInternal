using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserCreatedEvent : IDomainEvent
{
    #region Constructors
    internal UserCreatedEvent(Guid id, DateTime createdDateTime)
    {
        Id = id;
        CreatedDateTime = createdDateTime;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public DateTime CreatedDateTime { get; }
    #endregion
}