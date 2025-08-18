namespace NATSInternal.Core.DbContext;

internal class ExpenseEntityConfiguration : IEntityTypeConfiguration<Expense>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Expense> entityBuilder)
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
        entityBuilder.HasQueryFilter(e => !e.IsDeleted);
    }
    #endregion
}