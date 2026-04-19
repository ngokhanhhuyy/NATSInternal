namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the country-related operations.
/// </summary>
public interface ICountryService
{
    /// <summary>
    /// Retrieves all available countries in the application.
    /// </summary>
    /// <returns>
    /// A <typeparamref name="Task{TResult}"/> representing the asynchronous operation, which
    /// result is a <see cref="List{T}"/> of DTOs containing the countries' information.
    /// </returns>
    Task<List<CountryResponseDto>> GetAllAsync();
}
