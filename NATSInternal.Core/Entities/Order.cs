namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(OrderEntityConfiguration))]
[Table("orders")]
internal class Order : AbstractEntity<Order>, IHasStatsEntity<Order, OrderUpdateHistoryData>
{
    #region Fields
    private User? _createdUser;
    private Customer? _customer;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("type")]
    [Required]
    public OrderType Type { get; set; }

    [Column("stats_datetime")]
    [Required]
    public DateTime StatsDateTime { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; protected set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("note")]
    [StringLength(HasStatsContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("customer_id")]
    [Required]
    public Guid CustomerId { get; set; }

    [Column("created_user_id")]
    [Required]
    public Guid CreatedUserId { get; set; }
    #endregion

    #region CachedProperties
    [Column("cached_product items_amount_before_vat")]
    [Required]
    public long CachedProductItemsAmountBeforeVat { get; protected set; }

    [Column("cached_product_items_vat_amount")]
    [Required]
    public long CachedProductItemsVatAmount { get; protected set; }

    [Column("cached_service_items_amount_before_vat")]
    [Required]
    public long CachedServiceItemsAmountBeforeVat { get; protected set; }

    [Column("cached_service_items_vat_amount")]
    [Required]
    public long CachedServiceItemsVatAmount { get; protected set; }
    #endregion

    #region ConcurrencyOperationTrackingProperties
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_createdUser))]
    public User CreatedUser
    {
        get => GetFieldOrThrowIfNull(_createdUser);
        set
        {
            CreatedUserId = value.Id;
            _createdUser = value;
        }
    }

    [BackingField(nameof(_customer))]
    public Customer Customer
    {
        get => GetFieldOrThrowIfNull(_customer);
        set
        {
            CustomerId = value.Id;
            _customer = value;
        }
    }

    public List<OrderItem> Items { get; protected set; } = new();
    public List<Photo> Photos { get; protected set; } = new();
    public List<UpdateHistory> UpdateHistories { get; protected set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public string? ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    [NotMapped]
    public long ProductAmountBeforeVat => Items.Sum(i => i.AmountBeforeVatPerUnit * i.Quantity);

    [NotMapped]
    public long ProductVatAmount => Items.Sum(i => i.VatAmountPerUnit * i.Quantity);

    [NotMapped]
    public long AmountBeforeVat => ProductAmountBeforeVat;

    [NotMapped]
    public long AmountAfterVat => ProductAmountBeforeVat + ProductVatAmount;

    [NotMapped]
    public long VatAmount => ProductVatAmount;

    [NotMapped]
    public DateTime? LastUpdatedDateTime => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => (DateTime?)uh.UpdatedDateTime)
        .LastOrDefault();

    [NotMapped]
    public User? LastUpdatedUser => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => uh.UpdatedUser)
        .LastOrDefault();

    [NotMapped]
    public long CachedAmountAfterVat
    {
        get
        {
            long productAmount = CachedProductItemsAmountBeforeVat + CachedProductItemsVatAmount;
            long serviceAmount = CachedServiceItemsAmountBeforeVat + CachedServiceItemsVatAmount;

            return productAmount + serviceAmount;
        }
    }

    [NotMapped]
    public static Expression<Func<Order, long>> AmountAfterVatExpression => (order) =>
        order.Items.Sum(oi => (oi.AmountBeforeVatPerUnit + oi.VatAmountPerUnit) * oi.Quantity);
    #endregion

    #region Methods
    public void UpdateCachedProperties()
    {
        CachedProductItemsAmountBeforeVat = 0;
        CachedProductItemsVatAmount = 0;
        CachedServiceItemsAmountBeforeVat = 0;
        CachedServiceItemsVatAmount = 0;

        foreach (OrderItem item in Items)
        {
            if (item.Type == OrderItemType.Product)
            {
                CachedProductItemsAmountBeforeVat += item.AmountBeforeVatPerUnit * item.Quantity;
                CachedProductItemsVatAmount += item.VatAmountPerUnit * item.Quantity;
            }
            else
            {
                CachedServiceItemsAmountBeforeVat += item.AmountBeforeVatPerUnit * item.Quantity;
                CachedServiceItemsVatAmount += item.VatAmountPerUnit * item.Quantity;
            }
        }
    }
    #endregion
}