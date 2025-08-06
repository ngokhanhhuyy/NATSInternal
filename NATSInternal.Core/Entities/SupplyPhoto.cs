namespace NATSInternal.Core.Entities;

internal class SupplyPhoto : IPhotoEntity<SupplyPhoto>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Url { get; set; }

    // Foreign keys
    [Required]
    public int SupplyId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual Supply Supply { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<SupplyPhoto> entityBuilder)
    {
        entityBuilder.HasKey(p => p.Id);
        entityBuilder.HasOne(p => p.Supply)
            .WithMany(s => s.Photos)
            .HasForeignKey(p => p.SupplyId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}