namespace NATSInternal.Core.Entities;

[Table("expense_payees")]
internal class ExpensePayee : IHasIdEntity<ExpensePayee>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

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
    public List<Expense> Expenses { get; private set; } = new();
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<ExpensePayee> entityBuilder)
    {
        entityBuilder
            .HasIndex(ep => ep.Name)
            .HasDatabaseName("IX__expense_payees__name")
            .IsUnique();
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}