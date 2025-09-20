using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserRemovedFromRoleEvent : IDomainEvent
{
    #region Constructors
    public UserRemovedFromRoleEvent(UserSnapshot beforeRemovalSnapshot, UserSnapshot afterRemovalSnapshot)
    {
        BeforeRemovalSnapshot = beforeRemovalSnapshot;
        AfterRemovalSnapshot = afterRemovalSnapshot;
    }
    #endregion
    
    #region Properties
    public UserSnapshot BeforeRemovalSnapshot { get; }
    public UserSnapshot AfterRemovalSnapshot { get; }
    #endregion
}