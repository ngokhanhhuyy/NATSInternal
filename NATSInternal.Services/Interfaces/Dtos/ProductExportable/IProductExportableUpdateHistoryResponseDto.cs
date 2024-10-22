namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableUpdateHistoryResponseDto<TItemDataDto>
    : IProductEngageableUpdateHistoryResponseDto<TItemDataDto>
    where TItemDataDto : IProductExportableItemUpdateHistoryDataDto;