using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Persistence.DbContext;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    #region Methods
    public void Configure(EntityTypeBuilder<User> entityBuilder)
    {
        // Relationships.
        entityBuilder.HasOne(u => u.CreatedUser)
            .WithMany()
            .HasForeignKey(u => u.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(u => u.LastUpdatedUser)
            .WithMany()
            .HasForeignKey(u => u.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(u => u.DeletedUser)
            .WithMany()
            .HasForeignKey(u => u.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        entityBuilder.HasMany(u => u.Roles)
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
                    .IsRequired(),
                userRole =>
                {
                    userRole.ToTable(nameof(UserRole).Pluralize());
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                });
        
        // Index.
        entityBuilder.HasIndex(p => p.UserName).IsUnique();
    }
    #endregion
}

file class UserRole
{
    #region Properties
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int RoleId { get; set; }
    #endregion
    
    #region NavigationProperties
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
    #endregion
}