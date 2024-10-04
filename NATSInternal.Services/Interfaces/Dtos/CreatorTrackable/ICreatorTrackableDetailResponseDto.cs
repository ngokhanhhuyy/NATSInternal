namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICreatorTrackableDetailResponseDto<TAuthorization>
    : IUpsertableDetailResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    UserBasicResponseDto CreatedUser { get; internal set; }
}