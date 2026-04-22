namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(ProductCategoryEntityConfiguration))]
[Table("product_categories")]
internal class ProductCategory : AbstractEntity<ProductCategory>, IUpsertableEntity<ProductCategory>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("name")]
    [Required]
    [StringLength(ProductCategoryContracts.NameMaxLength)]
    public required string Name { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();
    #endregion

    #region NavigationProperties
    public List<Product> Products { get; protected set; } = new();
    #endregion
}