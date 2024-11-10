namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasProductUpdateHistoryResponseDto<TItemDataDto>
    : IHasStatsUpdateHistoryResponseDto
    where TItemDataDto : IHasProductItemUpdateHistoryDataDto
{
    List<TItemDataDto> OldItems { get; }
    List<TItemDataDto> NewItems { get; }
}