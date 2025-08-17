namespace NATSInternal.Core.Entities;

[Table("order_items")]
internal class OrderItem : AbstractHasIdEntity<OrderItem>
{
    #region Fields
    private Order? _order;
    private Product? _product;
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
    public static void ConfigureModel(EntityTypeBuilder<OrderItem> entityBuilder)
    {
        entityBuilder.HasKey(oi => oi.Id);
        entityBuilder
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .HasConstraintName("FK__order_items__orders__order_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .HasConstraintName("FK__order_items__products__product_id")
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder
            .HasIndex(o => o.Type)
            .HasDatabaseName("IX__order_items__type");
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}