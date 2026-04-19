namespace NATSInternal.Core.DbContext;

internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
{
    #region Methods
    public void Configure(EntityTypeBuilder<OrderItem> entityBuilder)
    {
        entityBuilder.HasKey(op => new { op.OrderId, op.ProductId });
        entityBuilder
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .HasConstraintName("FK__order_products__orders__order_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .HasConstraintName("FK__order_products__products__product_id")
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder
            .HasIndex(o => o.Type)
            .HasDatabaseName("IX__order_items__type");
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}
