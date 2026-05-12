using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Debts;

namespace NATSInternal.Core.Persistence.DbContext;

internal class DebtEntityConfiguration : IEntityTypeConfiguration<Debt>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Debt> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entityBuilder
            .HasOne(o => o.CreatedUser)
            .WithMany()
            .HasForeignKey(o => o.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder.HasOne(o => o.LastUpdatedUser)
            .WithMany()
            .HasForeignKey(o => o.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(o => o.DeletedUser)
            .WithMany()
            .HasForeignKey(o => o.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes.
        entityBuilder.HasIndex(o => o.StatsDate);
        entityBuilder.HasIndex(o => o.CreatedDateTime);
        entityBuilder.HasIndex(o => o.LastUpdatedDateTime);

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}