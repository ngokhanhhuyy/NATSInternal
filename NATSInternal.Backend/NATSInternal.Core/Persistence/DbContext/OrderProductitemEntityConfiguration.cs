using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Orders;

namespace NATSInternal.Core.Persistence.DbContext;

internal class OrderProductItemEntityConfiguration : IEntityTypeConfiguration<OrderProductItem>
{
    #region Methods
    public void Configure(EntityTypeBuilder<OrderProductItem> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne(opi => opi.Order)
            .WithMany(o => o.ProductItems)
            .HasForeignKey(opi => opi.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Indexes.
        entityBuilder.HasIndex(opi => new { opi.OrderId, opi.ProductId }).IsUnique();

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}