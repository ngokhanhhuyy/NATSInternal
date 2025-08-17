namespace NATSInternal.Core.Entities;

[Table("expenses")]
internal class Expense
    :
        AbstractHasStatsEntity<Expense>,
        ICostEntity<Expense, ExpenseUpdateHistory, ExpenseUpdateHistoryData>,
        IHasPhotosEntity<Expense, ExpensePhoto>
{
    #region Fields
    private User? _createdUser;
    private ExpensePayee? _payee;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("amount")]
    [Required]
    public long Amount { get; set; }

    [Column("stats_datetime")]
    [Required]
    public DateTime StatsDateTime { get; set; }

    [Column("category")]
    [Required]
    public ExpenseCategory Category { get; set; }

    [Column("note")]
    [StringLength(ExpenseContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("created_user_id")]
    [Required]
    public Guid CreatedUserId { get; set; }

    [Column("payee_id")]
    [Required]
    public Guid PayeeId { get; set; }
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

    [BackingField(nameof(_payee))]
    public ExpensePayee Payee
    {
        get => GetFieldOrThrowIfNull(_payee);
        set
        {
            PayeeId = value.Id;
            _payee = value;
        }
    }

    public List<ExpensePhoto> Photos { get; private set; } = new();
    public List<ExpenseUpdateHistory> UpdateHistories { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public string? ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

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
    public static void ConfigureModel(EntityTypeBuilder<Expense> entityBuilder)
    {
        entityBuilder.HasKey(ex => ex.Id);
        entityBuilder
            .HasOne(ex => ex.CreatedUser)
            .WithMany(u => u.Expenses)
            .HasForeignKey(ex => ex.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK__expenses__users__created_user_id")
            .IsRequired();
        entityBuilder
            .HasOne(ex => ex.Payee)
            .WithMany(exp => exp.Expenses)
            .HasForeignKey(exp => exp.PayeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK__expenses__expense_payees__payee_id")
            .IsRequired();
        entityBuilder
            .HasIndex(e => e.StatsDateTime)
            .HasDatabaseName("IX__expenses__stats_datetime");
        entityBuilder
            .HasIndex(e => e.IsDeleted)
            .HasDatabaseName("IX__expenses__is_deleted");
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}