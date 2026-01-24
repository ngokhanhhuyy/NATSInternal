namespace NATSInternal.Application.Authorization;

public class BrandExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanEdit { get; internal set; }
    public bool CanDelete { get; internal set; }
    #endregion
}