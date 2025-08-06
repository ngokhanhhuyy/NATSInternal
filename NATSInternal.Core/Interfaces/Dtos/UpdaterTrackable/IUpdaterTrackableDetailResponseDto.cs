namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpdaterTrackableDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    : IUpsertableDetailResponseDto<TExistingAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    List<TUpdateHistory> UpdateHistories { get; }
}