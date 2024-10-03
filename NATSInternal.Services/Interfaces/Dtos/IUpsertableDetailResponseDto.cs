namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableDetailResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    DateTime CreatedDateTime { get; internal set; }
    UserBasicResponseDto CreatedUser { get; internal set; }
    DateTime LastUpdatedDateTime { get; internal set; }
}