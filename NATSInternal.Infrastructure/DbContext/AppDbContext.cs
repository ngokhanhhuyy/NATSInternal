using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Constructors
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<User> Users { get; private set; }
    public DbSet<User> Roles { get; private set; }
    public DbSet<User> Permissions { get; private set; }
    #endregion

    #region ProtectedMethods
    #endregion
}
