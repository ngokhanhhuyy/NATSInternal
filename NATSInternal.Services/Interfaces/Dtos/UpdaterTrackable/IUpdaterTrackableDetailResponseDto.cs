namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpdaterTrackableDetailResponseDto<TUpdateHistory, TAuthorization>
    : IUpsertableDetailResponseDto<TAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    List<TUpdateHistory> UpdateHistories { get; set; }
}