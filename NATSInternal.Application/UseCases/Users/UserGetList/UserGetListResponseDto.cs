using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetListResponseDto : IPageableListResponseDto<UserGetListUserResponseDto>
{
    #region Constructors
    internal UserGetListResponseDto(
        ICollection<User> users,
        int pageCount,
        Func<User, UserExistingAuthorizationResponseDto> authorizationGetter)
    {
        Items = users.Select(u => new UserGetListUserResponseDto(u, authorizationGetter(u))).ToList();
        PageCount = pageCount;
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