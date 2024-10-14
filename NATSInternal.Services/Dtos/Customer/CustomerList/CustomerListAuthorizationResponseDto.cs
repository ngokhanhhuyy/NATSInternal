namespace NATSInternal.Services.Dtos;

public class CustomerListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}
