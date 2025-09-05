using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    #region Methods
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary key.
        builder.HasKey(u => u.Id);

        // Relationship.
        builder
            .HasMany<Role>("_roles")
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                left => left
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                right => right
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                join =>
                {
                    join.HasKey("UserId, RoleId");
                    join.ToTable("UserRoles");
                });
        

        // Index.
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.NormalizedUserName).IsUnique();
        
        // Filter.
        builder.HasQueryFilter(u => u.DeletedDateTime == null);

        // Properties.
        builder
            .Property(u => u.UserName)
            .HasMaxLength(UserContracts.UserNameMaxLength)
            .IsRequired();
        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(UserContracts.UserNameMaxLength)
            .IsRequired();
        builder.Property(u => u.PasswordHash)
            .HasMaxLength(UserContracts.PasswordHashMaxLength)
            .IsRequired();
        builder
            .Property(u => u.CreatedDateTime)
            .IsRequired();

        // Ignore.
        builder.Ignore(u => u.Roles);
        builder.Ignore(u => u.PowerLevel);
    }
    #endregion
}