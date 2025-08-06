namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IExportProductBasicResponseDto<TExistingAuthorization>
    : IHasCustomerBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto;