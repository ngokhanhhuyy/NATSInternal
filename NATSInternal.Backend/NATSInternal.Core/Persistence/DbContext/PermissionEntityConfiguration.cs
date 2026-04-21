using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Persistence.DbContext;

internal class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Permission> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        // Indexes.
        entityBuilder.HasIndex(p => new { p.Name, p.RoleId }).IsUnique();
    }
    #endregion
}