using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Users;

internal class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Primary key.
        builder.HasKey(r => r.Id);

        // Relationships.
        builder
            .HasOne<Role>()
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Index.
        builder.HasIndex(r => r.Name);

        // Properties.
        builder.Property(r => r.Name).HasMaxLength(PermissionContracts.NameMaxLength).IsRequired();
    }
    #endregion
}