using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Shared;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetListResponseDto : IPageableListResponseDto<UserGetListUserResponseDto>
{
    #region Constructors
    internal UserGetListResponseDto(Page<User> page, Func<User, UserExistingAuthorizationResponseDto> authorizationGetter)
    {
        Items = page.Items.Select(u => new UserGetListUserResponseDto(u, authorizationGetter(u))).ToList();
        PageCount = page.PageCount;
    }
    #endregion
    
    #region Properties
    public ICollection<UserGetListUserResponseDto> Items { get; }
    public int PageCount { get; }
    #endregion
}

public class UserGetListUserResponseDto
{
    #region Constructors
    internal UserGetListUserResponseDto(User user, UserExistingAuthorizationResponseDto authorizationResponseDto)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new UserListRoleResponseDto(r)).ToList();
        Authorization = authorizationResponseDto;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string UserName { get; }
    public ICollection<UserListRoleResponseDto> Roles { get; }
    public UserExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}

public class UserListRoleResponseDto
{
    #region Constructors
    internal UserListRoleResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string DisplayName { get; }
    #endregion
}