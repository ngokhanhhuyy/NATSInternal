using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetDetailResponseDto
{
    #region Constructors
    internal UserGetDetailResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new UserGetDetailRoleResponseDto(r)).ToList();
    }

    internal UserGetDetailResponseDto(
        User user,
        UserExistingAuthorizationResponseDto authorizationResponseDto) : this(user)
    {
        Authorization = authorizationResponseDto;
    }
    #endregion

    #region Properties
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public List<UserGetDetailRoleResponseDto> Roles { get; set; }
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
