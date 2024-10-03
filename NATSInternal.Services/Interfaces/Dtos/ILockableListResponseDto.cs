namespace NATSInternal.Services.Interfaces.Dtos;

public interface ILockableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    : IUpsertableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    where TBasic : ILockableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    List<MonthYearResponseDto> MonthYearOptions { get; internal set; }
}