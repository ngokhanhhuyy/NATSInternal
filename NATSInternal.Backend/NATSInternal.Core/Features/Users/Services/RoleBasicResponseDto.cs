namespace NATSInternal.Core.Features.Users;

public class RoleBasicResponseDto
{
    #region Constructors
    internal RoleBasicResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public string DisplayName { get; }
    #endregion
}