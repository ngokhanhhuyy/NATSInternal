namespace NATSInternal.Core.Dtos;

public class BrandInitialResponseDto :
        IUpsertableInitialResponseDto,
        IHasOptionsInitialResponseDto<BrandMinimalResponseDto>

{
    #region Properties
    public string DisplayName { get; } = DisplayNames.Brand;
    public required List<BrandMinimalResponseDto> AllAsOptions { get; init; }
    public required bool CreatingPermission { get; init; }
    #endregion
}