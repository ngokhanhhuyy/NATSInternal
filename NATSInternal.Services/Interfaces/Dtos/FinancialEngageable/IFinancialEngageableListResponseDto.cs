namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableListResponseDto<
        TBasic,
        TAuthorization,
        TListAuthorization>
    : IUpsertableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    where TBasic : IFinancialEngageableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TListAuthorization : IUpsertableListAuthorizationResponseDto
{
    List<MonthYearResponseDto> MonthYearOptions { get; internal set; }
}