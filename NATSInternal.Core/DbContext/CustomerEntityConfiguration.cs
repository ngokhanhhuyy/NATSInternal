namespace NATSInternal.Core.DbContext;

internal class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Customer> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder
            .HasOne(c => c.Introducer)
            .WithMany(i => i.IntroducedCustomers)
            .HasForeignKey(c => c.IntroducerId)
            .HasConstraintName("FK__customers__customers__introducer_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasOne(c => c.CreatedUser)
            .WithMany(u => u.CreatedCustomers)
            .HasForeignKey(c => c.CreatedUserId)
            .HasConstraintName("FK__customers__users__created_user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(c => c.IsDeleted)
            .HasDatabaseName("IX__customers__is_deleted");
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
        entityBuilder.HasQueryFilter(c => !c.IsDeleted);
    }
    #endregion
}
