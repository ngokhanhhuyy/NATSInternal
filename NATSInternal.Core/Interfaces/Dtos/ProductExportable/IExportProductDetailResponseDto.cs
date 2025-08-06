namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IExportProductDetailResponseDto<
        TItem,
        TPhoto,
        TUpdateHistory,
        TItemUpdateHistoryData,
        TExistingAuthorization>
    :
        IHasProductDetailResponseDto<
            TItem,
            TPhoto,
            TUpdateHistory,
            TItemUpdateHistoryData,
            TExistingAuthorization>
    where TItem : IHasProductItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IProductExportableUpdateHistoryResponseDto<TItemUpdateHistoryData>
    where TItemUpdateHistoryData : IProductExportableItemUpdateHistoryDataDto
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    long AmountBeforeVat { get; }
    long VatAmount { get; }
}