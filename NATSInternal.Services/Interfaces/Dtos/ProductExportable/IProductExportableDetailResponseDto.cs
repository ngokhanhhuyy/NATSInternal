namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableDetailResponseDto<
            TCustomer,
            TProductItem,
            TPhoto,
            TUpdateHistory,
            TAuthorization,
            TCustomerAuthorazation>
    :
        IProductEngageableDetailResponseDto<
            TProductItem,
            TPhoto,
            TUpdateHistory,
            TAuthorization>,
        IRevuenueBasicResponseDto<TCustomer, TAuthorization, TCustomerAuthorazation>
    where TCustomer : ICustomerBasicResponseDto<TCustomerAuthorazation>
    where TProductItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization: IFinancialEngageableAuthorizationResponseDto
    where TCustomerAuthorazation : IUpsertableAuthorizationResponseDto
{
}