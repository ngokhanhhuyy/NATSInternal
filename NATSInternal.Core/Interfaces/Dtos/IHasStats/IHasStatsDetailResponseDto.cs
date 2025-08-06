namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasStatsDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    :
        ICreatorTrackableDetailResponseDto<TExistingAuthorization>,
        IUpdaterTrackableDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    where TUpdateHistory : IHasStatsUpdateHistoryResponseDto
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    DateTime StatsDateTime { get; }
    string Note { get; }
    bool IsLocked { get; }
}