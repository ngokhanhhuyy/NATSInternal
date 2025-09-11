using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserAddedToRoleEvent : IDomainEvent
{
    #region Constructors
    public UserAddedToRoleEvent(Guid addedUserId, Guid addedRoleId, string addedRoleName)
    {
        AddedUserId = addedUserId;
        AddedRoleId = addedRoleId;
        AddedRoleName = addedRoleName;
    }
    #endregion
    
    #region Properties
    public Guid AddedUserId { get; }
    public Guid AddedRoleId { get; }
    public string AddedRoleName { get; }
    #endregion
}