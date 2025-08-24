namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class RoleService : IRoleService
{
    #region Fields
    private readonly DatabaseContext _context;
    #endregion

    #region Constructors
    public RoleService(DatabaseContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<List<RoleMinimalResponseDto>> GetAllAsync()
    {
        return await _context.Roles
            .Select(role => new RoleMinimalResponseDto(role))
            .ToListAsync();
    }
    #endregion
}