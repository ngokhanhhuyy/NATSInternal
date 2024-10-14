namespace NATSInternal.Services.Dtos;

internal class ProductCategoryListAuthorizationResponseDto
    : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}
