namespace NATSInternal.Core.Dtos;

public class CustomerInitialResponseDto
    : IUpsertableInitialResponseDto, ISortableInitialResponseDto
{
    public string DisplayName => DisplayNames.Customer;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; init; }
    public required bool CreatingPermission { get; init; }
}