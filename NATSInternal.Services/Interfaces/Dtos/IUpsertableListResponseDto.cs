namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableListResponseDto<TBasic, TAuthorization, TListAuthorization>
    : IListResponseDto<TBasic>
    where TBasic : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    TAuthorization Authorization { get; internal set; }
}