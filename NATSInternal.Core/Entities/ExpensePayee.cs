namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(ExpensePayeeEntityConfiguration))]
[Table("expense_payees")]
internal class ExpensePayee : IHasIdEntity<ExpensePayee>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("name")]
    [Required]
    [StringLength(ExpensePayeeContracts.NameMaxLength)]
    public required string Name { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperties
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    public List<Expense> Expenses { get; protected set; } = new();
    #endregion
}