namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpsertableDetailResponseDto<TExistingAuthorization>
    : IUpsertableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    DateTime CreatedDateTime { get; }
}