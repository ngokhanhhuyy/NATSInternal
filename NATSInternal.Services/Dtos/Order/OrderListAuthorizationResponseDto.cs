namespace NATSInternal.Services.Dtos;

public class OrderListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}