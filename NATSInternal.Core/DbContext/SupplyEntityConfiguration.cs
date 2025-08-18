namespace NATSInternal.Core.DbContext;

internal class SupplyEntityConfiguration : IEntityTypeConfiguration<Supply>
{
    #region Properties
    public void Configure(EntityTypeBuilder<Supply> entityBuilder)
    {
        entityBuilder.HasKey(s => s.Id);
        entityBuilder
            .HasOne(s => s.CreatedUser)
            .WithMany(u => u.Supplies)
            .HasForeignKey(s => s.CreatedUserId)
            .HasConstraintName("FK__supplies__users__created_user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(s => s.StatsDateTime)
            .IsUnique()
            .HasDatabaseName("IX__supplies__stats_datetime");
        entityBuilder
            .HasIndex(s => s.IsDeleted)
            .HasDatabaseName("IX__supplies__is_deleted");
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
        entityBuilder.HasQueryFilter(s => !s.IsDeleted);
    }
    #endregion
}