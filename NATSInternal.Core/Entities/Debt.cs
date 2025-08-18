namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(DebtEntityConfiguration))]
[Table("debts")]
internal class Debt : AbstractEntity<Debt>, IDebtEntity<Debt, DebtUpdateHistoryData>
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
    public DebtType Type { get; set; }

    [Column("stats_datetime")]
    [Required]
    public DateTime StatsDateTime { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; protected set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("amount")]
    [Required]
    public long Amount { get; set; }

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

    public List<UpdateHistory> UpdateHistories { get; protected set; } = new();
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
}
