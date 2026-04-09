namespace NATSInternal.Core.DbContext;

internal class ExpensePayeeEntityConfiguration : IEntityTypeConfiguration<ExpensePayee>
{
    #region Methods
    public void Configure(EntityTypeBuilder<ExpensePayee> entityBuilder)
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