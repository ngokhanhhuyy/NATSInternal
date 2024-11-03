namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableDetailResponseDto<
        TItem,
        TPhoto,
        TUpdateHistory,
        TItemUpdateHistoryData,
        TExistingAuthorization>
    :
        IProductEngageableDetailResponseDto<
            TItem,
            TPhoto,
            TUpdateHistory,
            TItemUpdateHistoryData,
            TExistingAuthorization>,
        ICustomerEngageableBasicResponseDto<TExistingAuthorization>
    where TItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IProductExportableUpdateHistoryResponseDto<TItemUpdateHistoryData>
    where TItemUpdateHistoryData : IProductExportableItemUpdateHistoryDataDto
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto
{
    long AmountBeforeVat { get; }
    long VatAmount { get; }
}