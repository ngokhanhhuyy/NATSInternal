using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Shared;

public class UserBasicResponseDto
{
    #region Constructors
    internal UserBasicResponseDto(User? user)
    {
        if (user is not null && !user.DeletedDateTime.HasValue)
        {
            Id = user.Id;
            UserName = user.UserName;
            IsDeleted = false;
        }
    }
    #endregion
    
    #region Properties
    public Guid Id { get; } = Guid.Empty;
    public string UserName { get; } = string.Empty;
    public bool IsDeleted { get; } = true;
    #endregion
}