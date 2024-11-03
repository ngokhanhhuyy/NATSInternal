namespace NATSInternal.Services.Interfaces.Dtos;

internal interface ICustomerEngageableBasicResponseDto<TExistingAuthorization>
    : IFinancialEngageableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto
{
    CustomerBasicResponseDto Customer { get; }
}