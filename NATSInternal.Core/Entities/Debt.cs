namespace NATSInternal.Core.Entities;

[Table("debts")]
internal class Debt
    :
        AbstractHasStatsEntity<Debt, DebtUpdateHistory, DebtUpdateHistoryData>,
        IDebtEntity<Debt, DebtUpdateHistory, DebtUpdateHistoryData>
{
    #region Fields
    private Customer? _customer;
    #endregion

    #region Properties
    [Column("type")]
    [Required]
    public DebtType Type { get; set; }

    [Column("amount")]
    [Required]
    public long Amount { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("customer_id")]
    [Required]
    public Guid CustomerId { get; set; }
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
