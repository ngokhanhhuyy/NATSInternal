namespace NATSInternal.Services.Interfaces.Dtos;

internal interface ISortableInitialResponseDto : IInitialResponseDto
{
    ListSortingOptionsResponseDto ListSortingOptions { get; }
}