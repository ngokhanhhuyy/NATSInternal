namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableDetailResponseDto<
        TUpdateHistory,
        TExistingAuthorization>
    :
        IFinancialEngageableBasicResponseDto<TExistingAuthorization>,
        ICreatorTrackableDetailResponseDto<TExistingAuthorization>,
        IUpdaterTrackableDetailResponseDto<TUpdateHistory, TExistingAuthorization>
    where TUpdateHistory : IFinancialEngageableUpdateHistoryResponseDto
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto
{
    string Note { get; }
}