using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Shared;

namespace NATSInternal.Application.UseCases.Users;

public class UserListResponseDto : IPageableListResponseDto<UserListUserResponseDto>
{
    #region Constructors
    public UserListResponseDto(Page<User> page)
    {
        Items = page.Items.Select(u => new UserListUserResponseDto(u)).ToList();
        PageCount = page.PageCount;
    }
    #endregion
    
    #region Properties
    public ICollection<UserListUserResponseDto> Items { get; }
    public int PageCount { get; }
    #endregion
}

public class UserListUserResponseDto
{
    #region Constructors
    public UserListUserResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new UserListRoleResponseDto(r)).ToList();
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string UserName { get; }
    public ICollection<UserListRoleResponseDto> Roles { get; }
    #endregion
}

public class UserListRoleResponseDto
{
    #region Constructors
    public UserListRoleResponseDto(Role role)
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