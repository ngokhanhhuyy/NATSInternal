namespace NATSInternal.Services.Dtos;

public class CountryInitialResponseDto : IHasOptionsInitialResponseDto<CountryResponseDto>
{
    public string DisplayName => DisplayNames.Country;
    public required List<CountryResponseDto> AllAsOptions { get; set; }
}