namespace NATSInternal.Services.Dtos;

public class ProductCategoryExistingAuthorizationResponseDto
        : IUpsertableExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
