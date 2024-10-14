namespace NATSInternal.Services.Dtos;

public class ProductCategoryAuthorizationResponseDto : IUpsertableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
