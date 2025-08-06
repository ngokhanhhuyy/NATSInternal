namespace NATSInternal.Core.Entities;

internal class OrderPhoto : IPhotoEntity<OrderPhoto>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Url { get; set; }

    // Foreign keys
    [Required]
    public int OrderId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationship
    public virtual Order Order { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<OrderPhoto> entityBuilder)
    {
        entityBuilder.HasKey(op => op.Id);
        entityBuilder.HasOne(p => p.Order)
            .WithMany(o => o.Photos)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasIndex(p => p.Url)
            .IsUnique();
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}