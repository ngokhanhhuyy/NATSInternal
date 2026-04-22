namespace NATSInternal.Core.Interfaces.Dtos;

internal interface ISortableInitialResponseDto : IInitialResponseDto
{
    ListSortingOptionsResponseDto ListSortingOptions { get; }
}