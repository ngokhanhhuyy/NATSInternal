namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasCustomerBasicResponseDto<TExistingAuthorization>
    : IHasStatsBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    CustomerBasicResponseDto Customer { get; }
}