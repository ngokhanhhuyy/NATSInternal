namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableDetailResponseDto<
        TItem,
        TPhoto,
        TUpdateHistory,
        TItemUpdateHistoryData,
        TAuthorization>
    :
        IProductEngageableDetailResponseDto<
            TItem,
            TPhoto,
            TUpdateHistory,
            TItemUpdateHistoryData,
            TAuthorization>,
        ICustomerEngageableBasicResponseDto<TAuthorization>
    where TItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IProductExportableUpdateHistoryResponseDto<TItemUpdateHistoryData>
    where TItemUpdateHistoryData : IProductExportableItemUpdateHistoryDataDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    long AmountBeforeVat { get; }
    long VatAmount { get; }
}