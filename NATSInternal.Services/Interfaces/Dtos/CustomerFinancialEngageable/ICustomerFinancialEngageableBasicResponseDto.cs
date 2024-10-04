namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerFinancialEngageableBasicResponseDto<TCustomer, TAuthorization>
    : IFinancialEngageableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    TCustomer Customer { get; internal set; }
}