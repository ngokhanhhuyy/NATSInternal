namespace NATSInternal.Core.Features.Metadata;

public class MetadataCreatingAuthorizationListResponseDto
{
    #region Properties
    public required bool CanCreateUser { get; init; }
    public required bool CanCreateCustomer { get; init; }
    public required bool CanCreateProduct { get; init; }
    public required bool CanCreateProductCategory { get; init; }
    #endregion
}
