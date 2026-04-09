using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Users;

internal class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Primary key.
        builder.HasKey(r => r.Id);

        // Index.
        builder.HasIndex(r => r.Name).IsUnique();

        // Properties.
        builder.Property(r => r.Name).HasMaxLength(RoleContracts.NameMaxLength).IsRequired();
        builder.Property(r => r.DisplayName).HasMaxLength(RoleContracts.DisplayNameMaxLength).IsRequired();
        builder.Property(r => r.PowerLevel).IsRequired();

        // Ignore.
        builder.Ignore(r => r.Permissions);
    }
    #endregion
}