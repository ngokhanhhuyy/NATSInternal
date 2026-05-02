using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Persistence.DbContext;

internal class StockEntityTypeConfiguration : IEntityTypeConfiguration<Stock>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Stock> entityBuilder)
    {
        // Relationship.
        entityBuilder
            .HasOne(s => s.Product)
            .WithOne(p => p.Stock)
            .HasForeignKey<Stock>(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}