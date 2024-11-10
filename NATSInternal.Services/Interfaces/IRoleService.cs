namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the role-related operations.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Get a list of all roles' basic information.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an a
    /// <see cref="List{T}"/> of <see cref="RoleMinimalResponseDto"/> DTOs, containing the
    /// results.
    /// </returns>
    Task<List<RoleMinimalResponseDto>> GetAllAsync();
}
