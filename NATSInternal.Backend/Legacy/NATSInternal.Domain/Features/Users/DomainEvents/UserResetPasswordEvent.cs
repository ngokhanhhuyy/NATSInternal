using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

public class UserResetPasswordEvent : IDomainEvent
{
    #region Constructors
    internal UserResetPasswordEvent(Guid id, DateTime resettedDateTime)
    {
        Id = id;
        ResettedDateTime = resettedDateTime;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public DateTime ResettedDateTime { get; }
    #endregion
}