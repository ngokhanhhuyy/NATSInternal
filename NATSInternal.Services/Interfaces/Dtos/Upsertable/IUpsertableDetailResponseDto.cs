namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableDetailResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    DateTime CreatedDateTime { get; set; }
}