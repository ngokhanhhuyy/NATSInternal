namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductEngageableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    : IFinancialEngageableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    where TBasic : IFinancialEngageableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TListAuthorization : IUpsertableListAuthorizationResponseDto
{
    int? ProductId { get; set; }
}