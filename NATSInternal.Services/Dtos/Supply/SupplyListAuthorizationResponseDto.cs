namespace NATSInternal.Services.Dtos;

public class SupplyListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}