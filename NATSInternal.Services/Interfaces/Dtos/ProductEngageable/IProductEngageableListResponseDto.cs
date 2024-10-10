namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductEngageableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    : IFinancialEngageableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    where TBasic : class, IFinancialEngageableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TListAuthorization : IUpsertableListAuthorizationResponseDto
{
    int? ProductId { get; }
}