namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableDetailResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    DateTime CreatedDateTime { get; }
    TExistingAuthorization? Authorization { get; }
}