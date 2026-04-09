namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IDebtDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    :
        IHasCustomerBasicResponseDto<TExistingAuthorization>,
        IHasStatsDetailResponseDto<
            TUpdateHistory,
            TExistingAuthorization>
    where TUpdateHistory : IDebtUpdateHistoryResponseDto
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto;