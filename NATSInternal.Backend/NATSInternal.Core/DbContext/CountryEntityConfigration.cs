namespace NATSInternal.Core.DbContext;

internal class CountryEntityConfigration : IEntityTypeConfiguration<Country>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Country> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder
            .HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("IX__countries__name");
        entityBuilder
            .HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX__countries__code");
    }
    #endregion
}
