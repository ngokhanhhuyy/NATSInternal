namespace NATSInternal.Core.Dtos;

public class UserInitialResponseDto
    :
        IUpsertableInitialResponseDto,
        ISortableInitialResponseDto
{
    public string DisplayName { get; } = DisplayNames.User;
    public required UserDetailResponseDto Detail { get; init; }
    public required ListSortingOptionsResponseDto ListSortingOptions { get; init; }
    public required bool CreatingPermission { get; init; }
}