using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

public class UserDetailResponseDto
{
    #region Constructors
    public UserDetailResponseDto(User user, UserDetailAuthorizationResponseDto authorizationResponseDto)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new UserDetailRoleResponseDto(r)).ToList();
        Authorization = authorizationResponseDto;
    }
    #endregion

    #region Properties
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public List<UserDetailRoleResponseDto> Roles { get; set; }
    public UserDetailAuthorizationResponseDto Authorization { get; set; }
    #endregion
}

public class UserDetailRoleResponseDto
{
    #region Constructors
    public UserDetailRoleResponseDto(Role role)
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

public class UserDetailAuthorizationResponseDto
{
    #region Properties
    public bool CanGetNote { get; set; }
    public bool CanEdit { get; set; }
    public bool CanEditUserPersonalInformation { get; set; }
    public bool CanEditUserUserInformation { get; set; }
    public bool CanAssignRole { get; set; }
    public bool CanChangePassword { get; set; }
    public bool CanResetPassword { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
