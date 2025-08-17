namespace NATSInternal.Core.Entities;

[Table("photos")]
internal class Photo : AbstractHasIdEntity<Photo>
{
    #region Fields
    private Product? _product;
    private Supply? _supply;
    private Expense? _expense;
    private Order? _order;
    #endregion

    #region Properties
    [Column("url")]
    [Required]
    [StringLength(255)]
    public required string Url { get; set; }

    [Column("is_thumbnail")]
    [Required]
    public bool IsThumbnail { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("product_id")]
    public Guid? ProductId { get; set; }

    [Column("supply_id")]
    public Guid? SupplyId { get; set; }

    [Column("expense_id")]
    public Guid? ExpenseId { get; set; }

    [Column("order_id")]
    public Guid? OrderId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperties
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_product))]
    public Product? Product
    {
        get => _product;
        set
        {
            ProductId = value?.Id;
            _product = value;
        }
    }

    [BackingField(nameof(_supply))]
    public Supply? Supply
    {
        get => _supply;
        set
        {
            SupplyId = value?.Id;
            _supply = value;
        }
    }

    [BackingField(nameof(_expense))]
    public Expense? Expense
    {
        get => _expense;
        set
        {
            ExpenseId = value?.Id;
            _expense = value;
        }
    }

    [BackingField(nameof(_order))]
    public Order? Order
    {
        get => _order;
        set
        {
            OrderId = value?.Id;
            _order = value;
        }
    }
    #endregion
}