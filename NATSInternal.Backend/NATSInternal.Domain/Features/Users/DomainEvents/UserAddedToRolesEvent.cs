using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserAddedToRolesEvent : IDomainEvent
{
    #region Constructors
    public UserAddedToRolesEvent(Guid id, DateTime addedDateTime)
    {
        Id = id;
        AddedDateTime = addedDateTime;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public DateTime AddedDateTime { get; }
    #endregion
}