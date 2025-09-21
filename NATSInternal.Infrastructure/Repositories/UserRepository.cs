using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<User?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.Entry(user).Property("NormalizedUserName").CurrentValue = ComputeNormalizedUserName(user.UserName);
    }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.Entry(user).Property("NormalizedUserName").CurrentValue = ComputeNormalizedUserName(user.UserName);
    }

    public async Task<List<Role>> GetRolesByNameAsync(
        IEnumerable<string> roleNames,
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles.Where(r => roleNames.Contains(r.Name)).ToListAsync(cancellationToken);
    }
    #endregion

    #region StaticMethods
    private string ComputeNormalizedUserName(string userName)
    {
        return userName.ToLower();
    }
    #endregion
}
