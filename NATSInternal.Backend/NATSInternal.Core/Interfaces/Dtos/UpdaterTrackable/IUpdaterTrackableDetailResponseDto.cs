namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpdaterTrackableDetailResponseDto<TUpdateHistoryData, TExistingAuthorization>
    :
        IUpsertableDetailResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    List<TUpdateHistoryData> UpdateHistories { get; }
    #endregion
}