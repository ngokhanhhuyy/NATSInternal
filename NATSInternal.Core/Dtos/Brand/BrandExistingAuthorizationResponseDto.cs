namespace NATSInternal.Core.Dtos;

public class BrandExistingAuthorizationResponseDto
        : IUpsertableExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
