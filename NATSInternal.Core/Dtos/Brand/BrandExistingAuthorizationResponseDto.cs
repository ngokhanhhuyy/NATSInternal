namespace NATSInternal.Core.Dtos;

public class BrandExistingAuthorizationResponseDto : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
