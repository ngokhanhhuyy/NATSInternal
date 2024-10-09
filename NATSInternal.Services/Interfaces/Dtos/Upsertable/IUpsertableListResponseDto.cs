namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpsertableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    : IListResponseDto<TBasic>
    where TBasic : class, IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
    where TListAuthorization : IUpsertableListAuthorizationResponseDto
{
    TListAuthorization Authorization { get; set; }
}