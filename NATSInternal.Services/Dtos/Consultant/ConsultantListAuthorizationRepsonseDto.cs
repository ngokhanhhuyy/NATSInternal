namespace NATSInternal.Services.Dtos;

public class ConsultantListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}