namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableDetailResponseDto<TUpdateHistory, TAuthorization>
    :
        IFinancialEngageableBasicResponseDto<TAuthorization>,
        ICreatorTrackableDetailResponseDto<TAuthorization>,
        IUpdaterTrackableDetailResponseDto<TUpdateHistory, TAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    string Note { get; }
}