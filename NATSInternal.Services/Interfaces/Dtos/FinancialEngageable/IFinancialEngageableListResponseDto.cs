namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableListResponseDto<
        TBasic,
        TExistingAuthorization>
    : IUpsertableListResponseDto<TBasic, TExistingAuthorization>
    where TBasic : class, IFinancialEngageableBasicResponseDto< TExistingAuthorization>
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto;