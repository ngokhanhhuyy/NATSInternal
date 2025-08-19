namespace NATSInternal.Core.Dtos;

public class AddToRolesRequestDto
{
    #region Properties
    public List<string> RoleNames { get; set; } = new();
    #endregion
}
