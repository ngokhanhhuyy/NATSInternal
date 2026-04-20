using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Persistence.DbContext;

internal class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Properties
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    #endregion
    
    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasOne(u => u.CreatedUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(u => u.LastUpdatedUser)
                .WithMany()
                .HasForeignKey(u => u.LastUpdatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(u => u.DeletedUser)
                .WithMany()
                .HasForeignKey(u => u.DeletedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>(
                    userRole => userRole
                        .HasOne(ur => ur.Role)
                        .WithMany()
                        .HasForeignKey(ur => ur.RoleId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    userRole => userRole
                        .HasOne(ur => ur.User)
                        .WithMany()
                        .HasForeignKey(ur => ur.UserId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired());
        });

        modelBuilder.Entity<Permission>()
            .HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
    #endregion
}