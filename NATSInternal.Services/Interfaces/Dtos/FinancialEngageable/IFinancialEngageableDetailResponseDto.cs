namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableDetailResponseDto<TUpdateHistory, TAuthorization>
    :
        IFinancialEngageableBasicResponseDto<TAuthorization>,
        ICreatorTrackableDetailResponseDto<TAuthorization>,
        IUpdaterTrackableDetailResponseDto<TUpdateHistory, TAuthorization>
    where TUpdateHistory : IFinancialEngageableUpdateHistoryResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    string Note { get; }
}