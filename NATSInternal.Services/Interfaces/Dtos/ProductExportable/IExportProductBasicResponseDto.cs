namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IExportProductBasicResponseDto<TExistingAuthorization>
    : IHasCustomerBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto;