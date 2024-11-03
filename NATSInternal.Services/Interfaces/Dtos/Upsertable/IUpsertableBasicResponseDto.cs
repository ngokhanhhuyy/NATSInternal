namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpsertableBasicResponseDto<TExistingAuthorization> : IBasicResponseDto
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    TExistingAuthorization Authorization { get; }
}