namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableDetailResponseDto<
        TUserBasic,
        TUpdateHistory,
        TAuthorization,
        TUserAuthorization>
    :
        IFinancialEngageableBasicResponseDto<TAuthorization>,
        ICreatorTrackableDetailResponseDto<TAuthorization>,
        IUpdaterTrackableDetailResponseDto<
            TUserBasic,
            TUpdateHistory,
            TAuthorization,
            TUserAuthorization>
    where TUserBasic : IUserBasicResponseDto<TUserAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto<TUserBasic, TUserAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TUserAuthorization : IUpsertableAuthorizationResponseDto
{
    string Note { get; }
}