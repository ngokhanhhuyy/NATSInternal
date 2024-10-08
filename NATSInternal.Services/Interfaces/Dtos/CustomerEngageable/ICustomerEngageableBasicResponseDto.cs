namespace NATSInternal.Services.Interfaces.Dtos;

internal interface ICustomerEngageableBasicResponseDto<TAuthorization>
    : IFinancialEngageableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    CustomerBasicResponseDto Customer { get; set; }
}