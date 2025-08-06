namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableBasicResponseDto<TExistingAuthorization> : IBasicResponseDto
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    TExistingAuthorization Authorization { get; }
}