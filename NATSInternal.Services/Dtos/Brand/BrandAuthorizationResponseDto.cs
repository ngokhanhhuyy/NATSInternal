namespace NATSInternal.Services.Dtos;

public class BrandAuthorizationResponseDto : IUpsertableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
