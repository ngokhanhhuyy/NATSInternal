namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductEngageableUpdateHistoryResponseDto<TItemDataDto>
    : IFinancialEngageableUpdateHistoryResponseDto
    where TItemDataDto : IProductEngageableItemUpdateHistoryDataDto
{
    List<TItemDataDto> OldItems { get; }
    List<TItemDataDto> NewItems { get; }
}