namespace NATSInternal.Services.Entities;

internal class Product
    :
        FinancialEngageableAbstractEntity,
        IHasSinglePhotoEntity<Product>,
        IHasMultiplePhotosEntity<Product, ProductPhoto>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [Required]
    [StringLength(12)]
    public string Unit { get; set; }

    [Required]
    public long DefaultPrice { get; set; }

    [Required]
    public int DefaultVatPercentage { get; set; }

    [Required]
    public bool IsForRetail { get; set; } = true;

    [Required]
    public bool IsDiscontinued { get; set; }
    
    public DateTime? UpdatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();
    
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }

    [Required]
    public int StockingQuantity { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    // Foreign keys
    public int? BrandId { get; set; }
    public int? CategoryId { get; set; }

    // Relationships
    public virtual Brand Brand { get; set; }
    public virtual ProductCategory Category { get; set; }
    public virtual List<SupplyItem> SupplyItems { get; set; }
    public virtual List<OrderItem> OrderItems { get; set; }
    public virtual List<TreatmentItem> TreatmentItems { get; set; }
    public virtual List<ProductPhoto> Photos { get; set; }

    public static void ConfigureModel(EntityTypeBuilder<Product> entityBuilder)
    {
        entityBuilder.HasKey(p => p.Id);
        entityBuilder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder.HasOne(p => p.Category)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder.HasIndex(p => p.Name)
            .IsUnique();
    }
}