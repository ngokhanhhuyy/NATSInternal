namespace NATSInternal.Services.Interfaces.Dtos;

public interface IDebtListResponseDto<
        TBasic,
        TCustomerBasic,
        TUserBasic,
        TUpdateHistory,
        TListAuthorization,
        TAuthorization,
        TUserAuthorization,
        TCustomerAuthorziation>
    : IFinancialEngageableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    where TBasic : IFinancialEngageableBasicResponseDto<TAuthorization>
    where TCustomerBasic : ICustomerBasicResponseDto<TCustomerAuthorziation>
    where TUserBasic : IUserBasicResponseDto<TUserAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TListAuthorization : IUpsertableListAuthorizationResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TUserAuthorization : IUpsertableAuthorizationResponseDto
    where TCustomerAuthorziation : IUpsertableAuthorizationResponseDto
{

}