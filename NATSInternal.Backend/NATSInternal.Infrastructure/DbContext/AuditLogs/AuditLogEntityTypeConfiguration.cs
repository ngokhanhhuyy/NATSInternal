using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.AuditLogs;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext.AuditLogs;

internal class AuditLogEntityTypeConfiguration : IEntityTypeConfiguration<AuditLog>
{
    #region Methods
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        // Primary key.
        builder.HasKey(al => al.Id);

        // Relationships.
        builder.HasOne<User>().WithMany().HasForeignKey(al => al.PerformedUserId).IsRequired();

        // Properties.
        builder.Property<string>("DataJsonBeforeModification").HasMaxLength(5000).IsRequired();
        builder.Property<string>("DataJsonAfterModification").HasMaxLength(5000).IsRequired();

        // Indexes.
        builder.HasIndex(al => al.ActionName);
        builder.HasIndex(al => al.LoggedDateTime);
    }
    #endregion
}
