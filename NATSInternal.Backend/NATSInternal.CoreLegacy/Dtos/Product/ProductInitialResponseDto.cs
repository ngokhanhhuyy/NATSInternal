namespace NATSInternal.Core.Dtos;

public class ProductInitialResponseDto : IUpsertableInitialResponseDto, ISortableInitialResponseDto
{
    #region Properties
    public string DisplayName { get; } = DisplayNames.Product;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; set; }
    public required bool CreatingPermission { get; init; }
    #endregion
}