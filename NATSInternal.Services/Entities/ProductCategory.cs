namespace NATSInternal.Services.Entities;

internal class ProductCategory : IUpsertableEntity<ProductCategory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    // Relationships
    public virtual List<Product> Products { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<ProductCategory> entityBuilder)
    {
        entityBuilder.HasKey(pc => pc.Id);
        entityBuilder.HasIndex(pc => pc.Name)
            .IsUnique();
    }
}