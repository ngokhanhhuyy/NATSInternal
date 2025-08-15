namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class CountryService : ICountryService
{
    private readonly DatabaseContext _context;

    public CountryService(DatabaseContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<CountryResponseDto>> GetAllAsync()
    {
        return await _context.Countries
            .OrderBy(c => c.Id)
            .Select(c => new CountryResponseDto(c))
            .ToListAsync();
    }
}
