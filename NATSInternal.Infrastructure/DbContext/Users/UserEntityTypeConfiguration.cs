using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Seedwork;

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
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserRoles",
                r => r.HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                u => u.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                j => j.HasKey("UserId", "RoleId"));

        // Filter.
        builder.HasQueryFilter(u => u.DeletedDateTime == null);

        // Properties.
        builder
            .Property(u => u.UserName)
            .HasMaxLength(UserContracts.UserNameMaxLength)
            .IsRequired();
        builder.Property<string>("NormalizedUserName")
            .HasMaxLength(UserContracts.UserNameMaxLength)
            .IsRequired();
        builder.Property(u => u.PasswordHash)
            .HasMaxLength(UserContracts.PasswordHashMaxLength)
            .IsRequired();
        builder
            .Property(u => u.CreatedDateTime)
            .IsRequired();

        // Ignore.
        builder.Ignore(u => u.PowerLevel);

        // Index.
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex("NormalizedUserName").IsUnique();
    }
    #endregion

    #region Classes
    private class UserRole
    {
        #region Properties
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        #endregion

        #region NavigationProperties
        #nullable disable
        public User User { get; set; }
        public Role Role { get; set; }
        #nullable enable
        #endregion
    }
    #endregion
}