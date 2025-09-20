namespace NATSInternal.Domain.Features.Users;

public class UserChangeDataDto
{
    #region Constructors
    internal UserChangeDataDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new UserChangeRoleDataDto(r)).ToList();
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string UserName { get; }
    public ICollection<UserChangeRoleDataDto> Roles { get; }
    #endregion
}

public class UserChangeRoleDataDto
{
    #region Constructors
    internal UserChangeRoleDataDto(Role role)
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
