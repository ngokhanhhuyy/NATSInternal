using Bogus.Extensions;

namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(ProductEntityConfiguration))]
[Table("products")]
internal class Product : AbstractEntity<Product>, IHasPhotosEntity<Product>
{
    #region Fields
    private string _name = string.Empty;
    private Brand? _brand;
    private ProductCategory? _category;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

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
    public DateTime CreatedDateTime { get; private set; } = DateTime.UtcNow.ToApplicationTime();

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

    public List<SupplyItem> SupplyItems { get; protected set; } = new();
    public List<OrderItem> OrderItems { get; protected set; } = new();
    public List<Photo> Photos { get; protected set; } = new();
    #endregion
}