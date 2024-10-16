namespace NATSInternal.Services.Entities;

internal class Order
    :   FinancialEngageableAbstractEntity,
        IProductExportableEntity<Order, OrderItem, OrderPhoto, OrderUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime StatsDateTime { get; set; }

    [StringLength(255)]
    public string Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    // Foreign keys
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int CreatedUserId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationships
    public virtual User CreatedUser { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual List<OrderItem> Items { get; set; }
    public virtual List<OrderPhoto> Photos { get; set; }
    public virtual List<OrderUpdateHistory> UpdateHistories { get; set; }

    // Property for convinience.
    [NotMapped]
    public long ProductAmountBeforeVat => Items.Sum(i => i.ProductAmountPerUnit * i.Quantity);

    [NotMapped]
    public long ProductVatAmount => Items.Sum(i => i.VatAmountPerUnit * i.Quantity);

    [NotMapped]
    public long AmountBeforeVat => ProductAmountBeforeVat;

    [NotMapped]
    public long AfterVatAmount => ProductAmountBeforeVat + ProductVatAmount;

    [NotMapped]
    public long VatAmount => ProductVatAmount;

    [NotMapped]
    public DateTime? LastUpdatedDateTime => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => (DateTime?)uh.UpdatedDateTime)
        .LastOrDefault();

    [NotMapped]
    public User LastUpdatedUser => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => uh.UpdatedUser)
        .LastOrDefault();

    [NotMapped]
    public static Expression<Func<Order, long>> AmountAfterVatExpression
    {
        get
        {
            return (Order o) => o.Items.Sum(
                ti => (ti.ProductAmountPerUnit + ti.VatAmountPerUnit) * ti.Quantity);
        }
    }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Order> entityBuilder)
    {
        entityBuilder.HasKey(o => o.Id);
        entityBuilder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(o => o.CreatedUser)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(o => o.StatsDateTime);
        entityBuilder.HasIndex(o => o.IsDeleted);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}