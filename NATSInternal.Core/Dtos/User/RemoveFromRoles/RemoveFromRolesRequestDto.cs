namespace NATSInternal.Core.Dtos;

public class RemoveFromRolesRequestDto
{
    #region Properties
    public List<string> RoleNames { get; set; } = new();
    #endregion
}
