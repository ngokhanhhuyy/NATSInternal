using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Users;

public class UserBasicResponseDto
{
    #region Constructors
    internal UserBasicResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new RoleBasicResponseDto(r)).ToList();
        IsDeleted = user.DeletedDateTime is not null;
    }

    internal UserBasicResponseDto(User user, UserExistingAuthorizationResponseDto authorization) : this(user)
    {
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string UserName { get; }
    List<RoleBasicResponseDto> Roles { get; }
    public bool IsDeleted { get; }
    public UserExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}