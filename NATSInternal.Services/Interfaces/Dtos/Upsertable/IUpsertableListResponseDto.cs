namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    : IListResponseDto<TBasic>
    where TBasic : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
    where TListAuthorization : IUpsertableListAuthorizationResponseDto
{
    TListAuthorization Authorization { get; internal set; }
}