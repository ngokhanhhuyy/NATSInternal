namespace NATSInternal.Core.Dtos;

public class ProductExistingAuthorizationResponseDto : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
