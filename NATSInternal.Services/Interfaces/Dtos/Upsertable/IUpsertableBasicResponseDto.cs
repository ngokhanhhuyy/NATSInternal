namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableBasicResponseDto<TAuthorizationResponseDto> : IBasicResponseDto
    where TAuthorizationResponseDto : IUpsertableAuthorizationResponseDto
{
    TAuthorizationResponseDto Authorization { get; set; }
}