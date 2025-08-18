namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasCustomerBasicResponseDto<TExistingAuthorization>
    : IHasStatsBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    #region Properties
    CustomerBasicResponseDto Customer { get; }
    #endregion
}