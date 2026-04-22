using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Shared;

public class RoleBasicResponseDto
{
    #region Constructors
    internal RoleBasicResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
        PowerLevel = role.PowerLevel;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string DisplayName { get; }
    public int PowerLevel { get; }
    #endregion
}