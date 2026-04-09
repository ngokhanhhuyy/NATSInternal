namespace NATSInternal.Core.Dtos;

public class UserRemoveFromRolesRequestDto : IRequestDto
{
    #region Properties
    public List<string> RoleNames { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
