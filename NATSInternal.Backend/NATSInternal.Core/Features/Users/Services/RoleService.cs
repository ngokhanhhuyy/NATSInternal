using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Features.Users;

[UsedImplicitly]
internal class RoleService : IRoleService
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public RoleService(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<List<RoleBasicResponseDto>> GetAllAsync()
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .Select(r => new RoleBasicResponseDto(r))
            .ToListAsync();
    }

    public async Task<RoleDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .Select(r => new RoleDetailResponseDto(r))
            .SingleOrDefaultAsync(r => r.Id == id)
            ?? throw new NotFoundException();
    }
    #endregion
}