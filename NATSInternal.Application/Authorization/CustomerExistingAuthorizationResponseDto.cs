namespace NATSInternal.Application.Authorization;

public class CustomerExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}