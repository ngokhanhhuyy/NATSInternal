namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableUpdateHistoryResponseDto<TItemDataDto>
    : IHasProductUpdateHistoryResponseDto<TItemDataDto>
    where TItemDataDto : IProductExportableItemUpdateHistoryDataDto;