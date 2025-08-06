namespace NATSInternal.Core;

/// <inheritdoc />
internal class RoleService : IRoleService
{
    private readonly DatabaseContext _context;

    public RoleService(DatabaseContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<RoleMinimalResponseDto>> GetAllAsync()
    {
        return await _context.Roles
            .Select(role => new RoleMinimalResponseDto(role))
            .ToListAsync();
    }
}