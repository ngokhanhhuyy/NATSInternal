namespace NATSInternal.Core.DbContext;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Role> entityBuilder)
    {
        entityBuilder.HasKey(r => r.Id);
        entityBuilder
            .HasIndex(r => r.Name)
            .IsUnique()
            .HasDatabaseName("IX__roles__name");
        entityBuilder
            .HasIndex(r => r.DisplayName)
            .IsUnique()
            .HasDatabaseName("IX__roles__display_name");
    }
    #endregion
}
