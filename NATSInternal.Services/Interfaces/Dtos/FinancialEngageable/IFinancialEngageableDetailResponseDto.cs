namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableDetailResponseDto<TUpdateHistory, TAuthorization>
    :
        IFinancialEngageableBasicResponseDto<TAuthorization>,
        ICreatorTrackableDetailResponseDto<TAuthorization>,
        IUpdaterTrackableDetailResponseDto<TUpdateHistory, TAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto;