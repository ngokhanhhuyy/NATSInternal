using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserAddedToRolesEvent : IDomainEvent
{
    #region Constructors
    public UserAddedToRolesEvent(UserSnapshot beforeAddingSnapshot, UserSnapshot afterAddingSnapshot)
    {
        BeforeAddingSnapshot = beforeAddingSnapshot;
        AfterAddingSnapshot = afterAddingSnapshot;
    }
    #endregion
    
    #region Properties
    public UserSnapshot BeforeAddingSnapshot { get; }
    public UserSnapshot AfterAddingSnapshot { get; }
    #endregion
}