namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableListResponseDto<
        TBasic,
        TAuthorization,
        TListAuthorization>
    : IUpsertableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    where TBasic : class, IFinancialEngageableBasicResponseDto< TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TListAuthorization : IUpsertableListAuthorizationResponseDto
{
    List<MonthYearResponseDto> MonthYearOptions { get; set; }
}