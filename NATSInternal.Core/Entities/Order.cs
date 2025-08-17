namespace NATSInternal.Core.Entities;

[Table("orders")]
internal class Order : AbstractHasStatsEntity<Order>
{
    #region Fields
    private User? _createdUser;
    private Customer? _customer;
    #endregion

    #region Properties
    [Column("type")]
    [Required]
    public OrderType Type { get; set; }

    [Column("note")]
    [StringLength(OrderContracts.NoteMaxLength)]
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

    public List<OrderItem> Items { get; private set; } = new();
    public List<Photo> Photos { get; private set; } = new();
    public List<OrderUpdateHistory> UpdateHistories { get; private set; } = new();
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
    public static Expression<Func<Order, long>> AmountAfterVatExpression => (order) =>
        order.Items.Sum(oi => (oi.AmountBeforeVatPerUnit + oi.VatAmountPerUnit) * oi.Quantity);
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Order> entityBuilder)
    {
        entityBuilder.HasKey(o => o.Id);
        entityBuilder
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder
            .HasOne(o => o.CreatedUser)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder
            .HasIndex(o => o.StatsDateTime);
        entityBuilder
            .HasIndex(o => o.IsDeleted);
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}