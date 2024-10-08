namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerEngageableBasicResponseDto<
        TCustomer,
        TAuthorization,
        TCustomerAuthorization>
    : IFinancialEngageableBasicResponseDto<TAuthorization>
    where TCustomer : ICustomerBasicResponseDto<TCustomerAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TCustomerAuthorization : IUpsertableAuthorizationResponseDto
{
    TCustomer Customer { get; set; }
}