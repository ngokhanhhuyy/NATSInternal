using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext;

internal class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Constructors
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<User> Users { get; private set; }
    public DbSet<Role> Roles { get; private set; }
    public DbSet<Permission> Permissions { get; private set; }
    #endregion

    #region ProtectedMethods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
    }
    #endregion
}
