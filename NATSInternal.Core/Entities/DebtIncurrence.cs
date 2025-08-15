namespace NATSInternal.Core.Entities;

internal class DebtIncurrence : HasStatsAbstractEntity, IDebtEntity<DebtIncurrence, DebtIncurrenceUpdateHistory>
{
    #region Fields
    private Customer? _customer;
    private User? _createdUser;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required]
    public long Amount { get; set; }

    [StringLength(DebtIncurrenceContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Required]
    public DateTime StatsDateTime { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public Guid CreatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_customer))]
    public Customer Customer
    {
        get => _customer ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(Customer)));
        set => _customer = value;
    }

    [BackingField(nameof(_createdUser))]
    public User CreatedUser
    {
        get => _createdUser ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(CreatedUser)));
        set => _createdUser = value;
    }

    public List<DebtIncurrenceUpdateHistory> UpdateHistories { get; private set; } = new();
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
    public static void ConfigureModel(EntityTypeBuilder<DebtIncurrence> entityBuilder)
    {
        entityBuilder.HasKey(d => d.Id);
        entityBuilder.HasOne(d => d.Customer)
            .WithMany(c => c.DebtIncurrences)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(d => d.CreatedUser)
            .WithMany(u => u.Debts)
            .HasForeignKey(d => d.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(d => d.StatsDateTime);
        entityBuilder.HasIndex(d => d.IsDeleted);
    }
    #endregion
}
