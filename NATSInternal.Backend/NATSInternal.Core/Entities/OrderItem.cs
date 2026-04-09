namespace NATSInternal.Core.Entities;

[Table("order_items")]
internal class OrderItem : AbstractEntity<OrderItemEntityConfiguration>
{
    #region Fields
    private Order? _order;
    private Product? _product;
    #endregion

    #region Constructors
    protected OrderItem() { }
    #endregion

    #region Properties
    [Column("name")]
    [StringLength(OrderItemContracts.NameMaxLength)]
    public string? Name { get; set; }

    [Column("type")]
    [Required]
    public OrderItemType Type { get; set; }

    [Column("amount_before_vat_per_unit")]
    [Required]
    public long AmountBeforeVatPerUnit { get; set; }

    [Column("vat_amount_per_unit")]
    [Required]
    public long VatAmountPerUnit { get; set; }

    [Column("quantity")]
    [Required]
    public int Quantity { get; set; } = 1;
    #endregion

    #region ForeignKeyProperties
    [Column("order_id")]
    [Required]
    public Guid OrderId { get; set; }

    [Column("product_id")]
    public Guid? ProductId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperties
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_order))]
    public Order Order
    {
        get => GetFieldOrThrowIfNull(_order);
        set
        {
            OrderId = value.Id;
            _order = value;
        }
    }

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
    #endregion

    #region StaticMethods
    public static OrderItem CreateForService(
        string name,
        long amountBeforeVatPerUnit,
        long vatAmountPerUnit,
        int quantity,
        Guid orderId)
    {
        return new()
        {
            Name = name,
            Type = OrderItemType.Service,
            AmountBeforeVatPerUnit = amountBeforeVatPerUnit,
            VatAmountPerUnit = vatAmountPerUnit,
            Quantity = quantity,
            OrderId = orderId
        };
    }

    public static OrderItem CreateForProduct(
        long amountBeforeVatPerUnit,
        long vatAmountPerUnit,
        int quantity,
        Guid orderId,
        Guid productId)
    {
        return new()
        {
            Type = OrderItemType.Product,
            AmountBeforeVatPerUnit = amountBeforeVatPerUnit,
            VatAmountPerUnit = vatAmountPerUnit,
            Quantity = quantity,
            OrderId = orderId,
            ProductId = productId
        };
    }
    #endregion
}