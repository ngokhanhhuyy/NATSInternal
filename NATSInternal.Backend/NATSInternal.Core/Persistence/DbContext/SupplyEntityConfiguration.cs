using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Supplies;

namespace NATSInternal.Core.Persistence.DbContext;

internal class SupplyEntityConfiguration : IEntityTypeConfiguration<Supply>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Supply> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne(p => p.CreatedUser)
            .WithMany()
            .HasForeignKey(p => p.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder.HasOne(p => p.LastUpdatedUser)
            .WithMany()
            .HasForeignKey(p => p.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(p => p.DeletedUser)
            .WithMany()
            .HasForeignKey(p => p.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes.
        entityBuilder.HasIndex(s => s.StatsDateTime);
        entityBuilder.HasIndex(s => s.CreatedDateTime);
        entityBuilder.HasIndex(s => s.LastUpdatedDateTime);
        entityBuilder.HasIndex(s => s.CachedItemsAmount);

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}