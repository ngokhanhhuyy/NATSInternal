namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(ExpenseEntityConfiguration))]
[Table("expenses")]
internal class Expense
    :
        AbstractEntity<Expense>,
        ICostEntity<Expense, ExpenseUpdateHistoryData>,
        IHasPhotosEntity<Expense>
{
    #region Fields
    private User? _createdUser;
    private ExpensePayee? _payee;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("amount")]
    [Required]
    public long Amount { get; set; }

    [Column("stats_datetime")]
    [Required]
    public DateTime StatsDateTime { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; protected set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("category")]
    [Required]
    public ExpenseCategory Category { get; set; }

    [Column("note")]
    [StringLength(HasStatsContracts.NoteMaxLength)]
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
}