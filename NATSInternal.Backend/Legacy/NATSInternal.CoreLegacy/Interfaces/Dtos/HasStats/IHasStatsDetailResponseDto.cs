namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasStatsDetailResponseDto<TUpdateHistoryData, TExistingAuthorization>
    :
        ICreatorTrackableDetailResponseDto<TExistingAuthorization>,
        IUpdaterTrackableDetailResponseDto<TUpdateHistoryData, TExistingAuthorization>
    where TUpdateHistoryData : IHasStatsUpdateHistoryDataResponseDto
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    #region Properties
    DateTime StatsDateTime { get; }
    string? Note { get; }
    bool IsLocked { get; }
    #endregion
}