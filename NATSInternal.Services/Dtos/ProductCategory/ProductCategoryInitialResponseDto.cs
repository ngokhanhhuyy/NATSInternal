namespace NATSInternal.Services.Dtos;

public class ProductCategoryInitialResponseDto
    :
        IUpsertableInitialResponseDto,
        IHasOptionsInitialResponseDto<ProductCategoryMinimalResponseDto>
{
    public string DisplayName { get; } = DisplayNames.Category;
    public required List<ProductCategoryMinimalResponseDto> AllAsOptions { get; init; }
    public required bool CreatingPermission { get; set; }
}
