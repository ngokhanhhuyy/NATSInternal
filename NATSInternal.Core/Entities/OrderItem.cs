namespace NATSInternal.Core.Entities;

internal class OrderItem : IExportProductItemEntity<OrderItem>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long ProductAmountPerUnit { get; set; }

    [Required]
    public long VatAmountPerUnit { get; set; }

    [Required]
    public int Quantity { get; set; } = 1;

    // Foreign keys
    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationships
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<OrderItem> entityBuilder)
    {
        entityBuilder.HasKey(oi => oi.Id);
        entityBuilder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}