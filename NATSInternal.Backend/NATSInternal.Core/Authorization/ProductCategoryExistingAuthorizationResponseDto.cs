namespace NATSInternal.Core.Common.Authorization;

public class ProductCategoryExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanEdit { get; internal set; }
    public bool CanDelete { get; internal set; }
    #endregion
}