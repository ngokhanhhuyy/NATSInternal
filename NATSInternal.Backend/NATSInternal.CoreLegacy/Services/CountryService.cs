namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class CountryService : ICountryService
{
    #region Fields
    private readonly DatabaseContext _context;
    #endregion

    #region Constructors
    public CountryService(DatabaseContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<List<CountryResponseDto>> GetAllAsync()
    {
        return await _context.Countries
            .OrderBy(c => c.Id)
            .Select(c => new CountryResponseDto(c))
            .ToListAsync();
    }
    #endregion
}
