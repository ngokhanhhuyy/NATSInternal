namespace NATSInternal.Core.Entities;

internal class ProductPhoto : IPhotoEntity<ProductPhoto>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Url { get; set; }

    // Foreign keys
    [Required]
    public int ProductId { get; set; }

    // Relationship
    public virtual Product Product { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<ProductPhoto> entityBuilder)
    {
        entityBuilder.HasKey(pp => pp.Id);
        entityBuilder.HasOne(photo => photo.Product)
            .WithMany(product => product.Photos)
            .HasForeignKey(photo => photo.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}