namespace NATSInternal.Services.Dtos;

public class ProductCategoryListAuthorizationResponseDto
    : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}
