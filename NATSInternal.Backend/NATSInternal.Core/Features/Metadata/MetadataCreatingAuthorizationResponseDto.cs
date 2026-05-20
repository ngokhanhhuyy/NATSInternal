namespace NATSInternal.Core.Features.Metadata;

public class MetadataCreatingAuthorizationListResponseDto
{
    #region Properties
    public required bool CanCreateUser { get; init; }
    public required bool CanCreateCustomer { get; init; }
    public required bool CanCreateProduct { get; init; }
    public required bool CanCreateProductCategory { get; init; }
    public required bool CanCreateExpense { get; init; }
    public required bool CanCreateSupply { get; init; }
    public required bool CanCreateOrder { get; init; }
    public required bool CanCreatePayment { get; init; }
    #endregion
}
