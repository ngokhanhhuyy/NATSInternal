namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpsertableDetailResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    DateTime CreatedDateTime { get; }
}