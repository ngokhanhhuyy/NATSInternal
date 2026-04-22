namespace NATSInternal.Application.Authorization;

public class ProductExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanEdit { get; internal set; }
    public bool CanDelete { get; internal set; }
    #endregion
}