namespace NATSInternal.Core.Dtos;

public class CountryInitialResponseDto : IHasOptionsInitialResponseDto<CountryResponseDto>
{
    #region Properties
    public string DisplayName { get; } = DisplayNames.Country;
    public required List<CountryResponseDto> AllAsOptions { get; set; }
    #endregion
}