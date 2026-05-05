using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Orders;

namespace NATSInternal.Core.Persistence.DbContext;

internal class OrderServiceItemEntityConfiguration : IEntityTypeConfiguration<OrderServiceItem>
{
    #region Methods
    public void Configure(EntityTypeBuilder<OrderServiceItem> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne(opi => opi.Order)
            .WithMany(o => o.ServiceItems)
            .HasForeignKey(opi => opi.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Indexes.
        entityBuilder.HasIndex(opi => new { opi.Name, opi.OrderId }).IsUnique();

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}