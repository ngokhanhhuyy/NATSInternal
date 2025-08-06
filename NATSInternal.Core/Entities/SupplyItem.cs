namespace NATSInternal.Core.Entities;

internal class SupplyItem : IHasProductItemEntity<SupplyItem>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long ProductAmountPerUnit { get; set; }

    [Required]
    public int Quantity { get; set; } = 1;

    // Foreign keys
    [Required]
    public int SupplyId { get; set; }

    [Required]
    public int ProductId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    [JsonIgnore]
    public byte[] RowVersion { get; set; }

    // Relationships
    [JsonIgnore]
    public virtual Supply Supply { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; }

    [JsonIgnore]
    public virtual List<OrderItem> OrderItems { get; set; }

    // Model configuration.
    public static void ConfigureModel(EntityTypeBuilder<SupplyItem> entityBuilder)
    {
        entityBuilder.HasKey(si => si.Id);
        entityBuilder.HasOne(si => si.Supply)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SupplyId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(si => si.Product)
            .WithMany(p => p.SupplyItems)
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.Property(si => si.RowVersion)
            .IsRowVersion();
    }
}