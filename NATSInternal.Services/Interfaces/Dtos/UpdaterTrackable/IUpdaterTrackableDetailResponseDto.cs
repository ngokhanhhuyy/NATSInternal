namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpdaterTrackableDetailResponseDto<
        TUserBasic,
        TUpdateHistory,
        TAuthorization,
        TUserAuthorization>
    : IUpsertableDetailResponseDto<TAuthorization>
    where TUserBasic : IUserBasicResponseDto<TUserAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto<TUserBasic, TUserAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
    where TUserAuthorization : IUpsertableAuthorizationResponseDto
{
    List<TUpdateHistory> UpdateHistories { get; set; }
}