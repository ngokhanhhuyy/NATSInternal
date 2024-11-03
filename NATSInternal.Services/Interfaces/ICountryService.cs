namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the country-related operations.
/// </summary>
public interface ICountryService
{    
    /// <summary>
    /// Retrieves a list of all countries with _names, which are available in the applccation.
    /// </summary>
    /// <returns>
    /// A <typeparamref name="Task{TResult}"/> representing the asynchronous operation, where
    /// <c>TResult</c> is an instance of the <see cref="CountryListResponseDto"/> class,
    /// containing the results.
    /// </returns>
    Task<CountryListResponseDto> GetListAsync();
}
