namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IDebtDetailResponseDto<TUpdateHistory, TAuthorization>
    :
        ICustomerEngageableBasicResponseDto<TAuthorization>,
        IFinancialEngageableDetailResponseDto<
            TUpdateHistory,
            TAuthorization>
    where TUpdateHistory : IDebtUpdateHistoryResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto;