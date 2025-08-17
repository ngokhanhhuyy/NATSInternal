namespace NATSInternal.Core.Entities;

[Table("consultants")]
internal class Consultant
    :
        AbstractHasStatsEntity<Consultant>,
        IRevenueEntity<Consultant, ConsultantUpdateHistory, ConsultantUpdateHistoryData>
{
    #region Fields
    private Customer? _customer;
    private User? _createdUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("stats_datetime")]
    [Required]
    public DateTime StatsDateTime { get; set; }

    [Column("amount_before_vat")]
    [Required]
    public long AmountBeforeVat { get; set; }

    [Column("vat_amount")]
    [Required]
    public long VatAmount { get; set; }

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
    public required Guid CustomerId { get; set; }

    [Column("created_user_id")]
    [Required]
    public Guid CreatedUserId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperties
    [Column("row_version")]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
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

    public List<ConsultantUpdateHistory> UpdateHistories { get; private set; } = new();
    #endregion

    #region ComputedProperties
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
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Consultant> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder
            .HasOne(cst => cst.Customer)
            .WithMany(ctm => ctm.Consultants)
            .HasForeignKey(cst => cst.CustomerId)
            .HasConstraintName("FK__consultants__customers__customer_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasOne(cst => cst.CreatedUser)
            .WithMany(u => u.Consultants)
            .HasForeignKey(cst => cst.CreatedUserId)
            .HasConstraintName("FK__consultants__users__created_user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(cst => cst.StatsDateTime)
            .HasDatabaseName("IX__consultants__stats_datetime");
        entityBuilder
            .HasIndex(cst => cst.IsDeleted)
            .HasDatabaseName("IX__consultants__is_deleted");
    }
    #endregion
}