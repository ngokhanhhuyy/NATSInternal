namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasStatsDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    :
        ICreatorTrackableDetailResponseDto<TExistingAuthorization>,
        IUpdaterTrackableDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    where TUpdateHistory : IHasStatsUpdateHistoryResponseDto
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    #region Properties
    DateTime StatsDateTime { get; }
    string Note { get; }
    bool IsLocked { get; }
    #endregion
}