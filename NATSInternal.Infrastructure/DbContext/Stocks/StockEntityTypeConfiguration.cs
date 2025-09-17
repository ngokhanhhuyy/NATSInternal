using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;

namespace NATSInternal.Infrastructure.DbContext;

internal class StockEntityTypeConfiguration : IEntityTypeConfiguration<Stock>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        // Primary key.
        builder.HasKey(s => s.Id);
        
        // Relationship.
        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        // Properties.
        builder.Property(s => s.StockingQuantity).IsRequired();
    }
    #endregion
}