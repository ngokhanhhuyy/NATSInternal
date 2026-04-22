using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserRemovedFromRoleEvent : IDomainEvent
{
    #region Constructors
    public UserRemovedFromRoleEvent(Guid id, DateTime removedDateTime)
    {
        Id = id;
        RemovedDateTime = removedDateTime;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public DateTime RemovedDateTime { get; }
    #endregion
}