using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Supplies;

namespace NATSInternal.Core.Persistence.DbContext;

internal class SupplyItemEntityConfiguration : IEntityTypeConfiguration<SupplyItem>
{
    #region Methods
    public void Configure(EntityTypeBuilder<SupplyItem> entityBuilder)
    {
        // Relationships.
        entityBuilder.HasOne(si => si.Supply)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SupplyId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        entityBuilder
            .HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Indexes.
        entityBuilder.HasIndex(si => new { si.ProductId, si.SupplyId }).IsUnique();

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}