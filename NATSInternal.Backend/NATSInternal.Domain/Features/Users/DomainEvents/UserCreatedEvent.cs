using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserCreatedEvent : IDomainEvent
{
    #region Constructors
    internal UserCreatedEvent(UserSnapshot userSnapshot)
    {
        Snapshot = userSnapshot;
    }
    #endregion
    
    #region Properties
    public UserSnapshot Snapshot { get; }
    #endregion
}