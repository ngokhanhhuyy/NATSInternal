using Bogus.Extensions;

namespace NATSInternal.Core.Entities;

[Table("products")]
internal class Product : AbstractHasIdEntity<Product>, IHasPhotosEntity<Product>
{
    #region Fields
    private string _name = string.Empty;
    private Brand? _brand;
    private ProductCategory? _category;
    #endregion

    #region Properties
    [Column("name")]
    [BackingField(nameof(_name))]
    [Required]
    [StringLength(ProductContracts.NameMaxLength)]
    public required string Name
    {
        get => _name;
        set
        {
            NormalizedName = value.ToUpper().RemoveDiacritics();
            _name = value;
        }
    }

    [Column("normalized_name")]
    [Required]
    [StringLength(ProductContracts.NameMaxLength)]
    public string NormalizedName { get; private set; } = string.Empty;

    [Column("description")]
    [StringLength(ProductContracts.DescriptionMaxLength)]
    public string? Description { get; set; }

    [Column("unit")]
    [Required]
    [StringLength(ProductContracts.UnitMaxLength)]
    public required string Unit { get; set; }

    [Column("default_amount_before_vat_per_unit")]
    [Required]
    public long DefaultAmountBeforeVatPerUnit { get; set; }

    [Column("default_vat_percentage")]
    [Required]
    public int DefaultVatPercentage { get; set; }

    [Column("is_for_retail")]
    [Required]
    public bool IsForRetail { get; set; } = true;

    [Column("is_discontinued")]
    [Required]
    public bool IsDiscontinued { get; set; }

    [Column("created_datetime")]
    public DateTime CreatedDateTime { get; private set; } = 

    [Column("last_updated_datetime")]
    public DateTime? LastUpdatedDateTime { get; set; }

    [Column("stocking_quantity")]
    [Required]
    public int StockingQuantity { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("brand_id")]
    public Guid? BrandId { get; set; }

    [Column("category_id")]
    public Guid? CategoryId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_brand))]
    public Brand? Brand
    {
        get => _brand;
        set
        {
            BrandId = value?.Id;
            _brand = value;
        }
    }

    [BackingField(nameof(_category))]
    public ProductCategory? Category
    {
        get => _category;
        set
        {
            CategoryId = value?.Id;
            _category = value;
        }
    }

    public List<OrderItem> OrderItems { get; private set; } = new();
    public List<Photo> Photos { get; private set; } = new();
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Product> entityBuilder)
    {
        entityBuilder.HasKey(p => p.Id);
        entityBuilder
            .HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .HasConstraintName("FK__products__brands__brand_id")
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder
            .HasOne(p => p.Category)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("FK__products__product_categories__category_id")
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder
            .HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName("IX__products__name");
    }
    #endregion
}