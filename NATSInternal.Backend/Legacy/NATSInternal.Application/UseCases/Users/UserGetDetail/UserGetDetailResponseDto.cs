using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetDetailResponseDto
{
    #region Constructors
    internal UserGetDetailResponseDto(User user, User? createdUser)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new UserGetDetailRoleResponseDto(r)).ToList();
        CreatedDateTime = user.CreatedDateTime;
        CreatedUser = new(createdUser);
    }

    internal UserGetDetailResponseDto(
        User user,
        User? createdUser,
        UserExistingAuthorizationResponseDto authorizationResponseDto) : this(user, createdUser)
    {
        Authorization = authorizationResponseDto;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string UserName { get; }
    public List<UserGetDetailRoleResponseDto> Roles { get; }
    public DateTime CreatedDateTime { get; internal set; }
    public UserBasicResponseDto CreatedUser { get; internal set; }
    public DateTime? LastUpdatedDateTime { get; internal set; }
    public UserBasicResponseDto? LastUpdatedUser { get; internal set; }
    public DateTime? DeletedDateTime { get; internal set; }
    public UserBasicResponseDto? DeletedUser { get; internal set; }
    public UserExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion
}

public class UserGetDetailRoleResponseDto
{
    #region Constructors
    internal UserGetDetailRoleResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
        PowerLevel = role.PowerLevel;
        PermissionNames = role.Permissions.Select(p => p.Name).ToList();
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string DisplayName { get; }
    public int PowerLevel { get; }
    public ICollection<string> PermissionNames { get; }
    #endregion
}
