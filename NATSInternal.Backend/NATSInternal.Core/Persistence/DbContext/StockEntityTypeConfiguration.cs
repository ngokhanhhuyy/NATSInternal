using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Persistence.DbContext;

internal class StockEntityTypeConfiguration : IEntityTypeConfiguration<Stock>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        // Relationship.
        builder
            .HasOne<Product>()
            .WithOne()
            .HasForeignKey<Stock>(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
    #endregion
}