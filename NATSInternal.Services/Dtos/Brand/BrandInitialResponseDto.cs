namespace NATSInternal.Services.Dtos;

public class BrandInitialResponseDto :
        IUpsertableInitialResponseDto,
        IHasOptionsInitialResponseDto<BrandMinimalResponseDto>

{
    public string DisplayName { get; } = DisplayNames.Brand;
    public required List<BrandMinimalResponseDto> AllAsOptions { get; init; }
    public required bool CreatingPermission { get; init; }
}