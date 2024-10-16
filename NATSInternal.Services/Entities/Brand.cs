namespace NATSInternal.Services.Entities;

internal class Brand : IHasSinglePhotoEntity<Brand>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(255)]
    public string Website { get; set; }

    [StringLength(1000)]
    public string SocialMediaUrl { get; set; }

    [StringLength(15)]
    public string PhoneNumber { get; set; }

    [StringLength(255)]
    public string Email { get; set; }

    [StringLength(255)]
    public string Address { get; set; }

    [StringLength(255)]
    public string ThumbnailUrl { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; }

    // Foreign keys
    public int? CountryId { get; set; }

    // Relationships
    public virtual List<Product> Products { get; set; }
    public virtual Country Country { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Brand> entityBuilder)
    {
        entityBuilder.HasKey(b => b.Id);
        entityBuilder.HasOne(b => b.Country)
            .WithMany(c => c.Brands)
            .HasForeignKey(b => b.CountryId)
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder.HasIndex(b => b.Name)
            .IsUnique();
    }
}