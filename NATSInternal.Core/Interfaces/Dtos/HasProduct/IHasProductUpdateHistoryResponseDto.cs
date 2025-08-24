namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasProductUpdateHistoryResponseDto<TItemDataDto> : IHasStatsUpdateHistoryDataResponseDto
    where TItemDataDto : IHasProductItemUpdateHistoryDataDto
{
    List<TItemDataDto> OldItems { get; }
    List<TItemDataDto> NewItems { get; }
}