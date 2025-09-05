using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserAddToRolesEvent : IDomainEvent
{
    #region Constructors
    public UserAddToRolesEvent(Guid addedUserId, List<Guid> addedRoleIds)
    {
        AddedUserId = addedUserId;
        AddedRoleIds = addedRoleIds;
    }
    #endregion
    
    #region Properties
    public Guid AddedUserId { get; }
    public List<Guid> AddedRoleIds { get; }
    #endregion
}