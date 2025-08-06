namespace NATSInternal.Core.Entities;

internal class TreatmentPhoto : IPhotoEntity<TreatmentPhoto>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Url { get; set; }

    [Required]
    public TreatmentPhotoType Type { get; set; }

    // Foreign key
    [Required]
    public int TreatmentId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationships
    public virtual Treatment Treatment { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<TreatmentPhoto> entityBuilder)
    {
        entityBuilder.HasKey(tp => tp.Id);
        entityBuilder.HasOne(p => p.Treatment)
            .WithMany(t => t.Photos)
            .HasForeignKey(p => p.TreatmentId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasIndex(p => p.Url)
            .IsUnique();
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}