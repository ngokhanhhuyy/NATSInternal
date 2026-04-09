using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserResetPasswordEvent : IDomainEvent
{
    #region Constructors
    internal UserResetPasswordEvent(UserSnapshot userSnapshot)
    {
        Snapshot = userSnapshot;
    }
    #endregion
    
    #region Properties
    public UserSnapshot Snapshot { get; }
    #endregion
}