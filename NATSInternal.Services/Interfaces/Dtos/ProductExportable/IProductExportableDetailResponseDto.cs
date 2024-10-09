namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableDetailResponseDto<
        TProductItem,
        TPhoto,
        TUpdateHistory,
        TAuthorization>
    :
        IProductEngageableDetailResponseDto<
            TProductItem,
            TPhoto,
            TUpdateHistory,
            TAuthorization>,
        IRevuenueBasicResponseDto<TAuthorization>
    where TProductItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization: IFinancialEngageableAuthorizationResponseDto
{
}