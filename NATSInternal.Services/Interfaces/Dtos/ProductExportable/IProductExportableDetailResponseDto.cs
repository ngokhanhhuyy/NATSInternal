namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductExportableDetailResponseDto<
            TCustomer,
            TProductItem,
            TPhoto,
            TUserBasic,
            TUpdateHistory,
            TAuthorization,
            TCustomerAuthorazation,
            TUserAuthorization>
    :
        IProductEngageableDetailResponseDto<
            TProductItem,
            TPhoto,
            TUserBasic,
            TUpdateHistory,
            TAuthorization,
            TUserAuthorization>,
        IRevuenueBasicResponseDto<TCustomer, TAuthorization, TCustomerAuthorazation>
    where TCustomer : ICustomerBasicResponseDto<TCustomerAuthorazation>
    where TProductItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUserBasic : IUserBasicResponseDto<TUserAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto<TUserBasic, TUserAuthorization>
    where TAuthorization: IFinancialEngageableAuthorizationResponseDto
    where TCustomerAuthorazation : IUpsertableAuthorizationResponseDto
{
}