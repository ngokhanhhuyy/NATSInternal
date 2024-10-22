namespace NATSInternal.Services.Entities;

internal class TreatmentItem : IProductExportableItemEntity<TreatmentItem>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long ProductAmountPerUnit { get; set; }

    [Required]
    public long VatAmountPerUnit { get; set; }

    [Required]
    public int Quantity { get; set; }

    // Foreign keys
    [Required]
    public int TreatmentId { get; set; }

    [Required]
    public int ProductId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationship
    public virtual Treatment Treatment { get; set; }
    public virtual Product Product { get; set; }

    // Properties for convinience.
    [NotMapped]
    public long AmountBeforeVat => ProductAmountPerUnit * Quantity;

    [NotMapped]
    public long VatAmount => VatAmountPerUnit * Quantity;

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<TreatmentItem> entityBuilder)
    {
        entityBuilder.HasKey(ti => ti.Id);
        entityBuilder.HasOne(ti => ti.Treatment)
            .WithMany(t => t.Items)
            .HasForeignKey(ti => ti.TreatmentId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(ti => ti.Product)
            .WithMany(p => p.TreatmentItems)
            .HasForeignKey(ti => ti.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}