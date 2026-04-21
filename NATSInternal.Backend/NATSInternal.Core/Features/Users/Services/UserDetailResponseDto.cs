using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Users;

public class UserDetailResponseDto
{
    #region Constructors
    internal UserDetailResponseDto(User user, UserExistingAuthorizationResponseDto authorization)
    {
        Id = user.Id;
        UserName = user.UserName;
        CreatedDateTime = user.CreatedDateTime;
        CreatedUser = new(user.CreatedUser);

        LastUpdatedDateTime = user.LastUpdatedDateTime;
        if (user.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(user.LastUpdatedUser);
        }

        DeletedDateTime = user.DeletedDateTime;
        if (user.DeletedUser is not null)
        {
            DeletedUser = new(user.DeletedUser);
        }

        Roles = user.Roles.Select(r => new RoleDetailResponseDto(r)).ToList();
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string UserName { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? DeletedDateTime { get; }
    public UserBasicResponseDto? DeletedUser { get; }
    public List<RoleDetailResponseDto> Roles { get; set; }
    public UserExistingAuthorizationResponseDto Authorization { get; set; }
    #endregion
}