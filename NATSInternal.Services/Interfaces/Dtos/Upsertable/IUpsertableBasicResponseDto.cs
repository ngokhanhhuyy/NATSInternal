namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpsertableBasicResponseDto<TAuthorization> : IBasicResponseDto
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    TAuthorization Authorization { get; set; }
}