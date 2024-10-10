namespace NATSInternal.Services.Interfaces.Dtos;

internal interface ICreatorTrackableDetailResponseDto<TAuthorization>
    : IUpsertableDetailResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    UserBasicResponseDto CreatedUser { get; }
}