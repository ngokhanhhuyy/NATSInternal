namespace NATSInternal.Core.DbContext;

internal class DebtEntityConfiguration : IEntityTypeConfiguration<Debt>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Debt> entityBuilder)
    {
        entityBuilder.HasKey(d => d.Id);
        entityBuilder
            .HasOne(d => d.Customer)
            .WithMany(c => c.Debts)
            .HasForeignKey(d => d.CustomerId)
            .HasConstraintName("FK__debts__customers__customer_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasOne(d => d.CreatedUser)
            .WithMany(u => u.Debts)
            .HasForeignKey(d => d.CreatedUserId)
            .HasConstraintName("FK__debts__users__created_user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(d => d.Type)
            .HasDatabaseName("IX__debts__type");
        entityBuilder
            .HasIndex(d => d.StatsDateTime)
            .HasDatabaseName("IX__debts__stats_datetime");
        entityBuilder
            .HasIndex(d => d.IsDeleted)
            .HasDatabaseName("IX__debts__is_deleted");
    }
    #endregion
}