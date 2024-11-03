namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IDebtDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    :
        ICustomerEngageableBasicResponseDto<TExistingAuthorization>,
        IFinancialEngageableDetailResponseDto<
            TUpdateHistory,
            TExistingAuthorization>
    where TUpdateHistory : IDebtUpdateHistoryResponseDto
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto;