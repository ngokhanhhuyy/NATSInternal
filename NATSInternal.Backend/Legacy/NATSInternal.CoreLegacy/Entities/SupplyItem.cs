namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(SupplyItemEntityConfiguration))]
[Table("supply_items")]
internal class SupplyItem : AbstractEntity<SupplyItem>, IHasProductItemEntity<SupplyItem>
{
    #region Fields
    private Supply? _supply;
    private Product? _product;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("amount_per_unit")]
    [Required]
    public long AmountPerUnit { get; set; }

    [Column("quantity")]
    [Required]
    public int Quantity { get; set; } = 1;
    #endregion
    
    #region ForeignKeyProperties
    [Column("supply_id")]
    [Required]
    public Guid SupplyId { get; set; }

    [Column("product_id")]
    [Required]
    public Guid ProductId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperties
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_supply))]
    public Supply Supply
    {
        get => GetFieldOrThrowIfNull(_supply);
        set
        {
            SupplyId = value.Id;
            _supply = value;
        }
    }

    [BackingField(nameof(_product))]
    public Product Product
    {
        get => GetFieldOrThrowIfNull(_product);
        set
        {
            ProductId = value.Id;
            _product = value;
        }
    }
    #endregion
}