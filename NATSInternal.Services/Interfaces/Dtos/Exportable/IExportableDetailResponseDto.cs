namespace NATSInternal.Services.Interfaces.Dtos;

public interface IExportableDetailResponseDto<
            TCustomer,
            TProductItem,
            TUpdateHistory,
            TAuthorization,
            TCustomerAuthorazation>
    :
        IProductEngageableDetailResponseDto<TProductItem, TUpdateHistory, TAuthorization>,
        IPayableBasicResponseDto<TCustomer, TAuthorization>
    where TCustomer : ICustomerBasicResponseDto<TCustomerAuthorazation>
    where TProductItem : IProductEngageableItemResponseDto
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization: IFinancialEngageableAuthorizationResponseDto
    where TCustomerAuthorazation : IUpsertableAuthorizationResponseDto
{
}