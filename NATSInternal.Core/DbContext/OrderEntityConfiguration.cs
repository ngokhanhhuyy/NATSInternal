namespace NATSInternal.Core.DbContext;

internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Order> entityBuilder)
    {
        entityBuilder.HasKey(o => o.Id);
        entityBuilder
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .HasConstraintName("FK__orders__customers__customer_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasOne(o => o.CreatedUser)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.CreatedUserId)
            .HasConstraintName("FK__orders__users__user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(o => o.StatsDateTime)
            .HasDatabaseName("IX__orders__stats_datetime");
        entityBuilder
            .HasIndex(o => o.IsDeleted)
            .HasDatabaseName("IX__orders__is_deleted");
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
        entityBuilder.HasQueryFilter(o => !o.IsDeleted);
    }
    #endregion
}
