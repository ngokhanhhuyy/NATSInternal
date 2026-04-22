namespace NATSInternal.Core.DbContext;

internal class BrandEntityConfiguration : IEntityTypeConfiguration<Brand>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Brand> entityBuilder)
    {
        entityBuilder.HasKey(b => b.Id);
        entityBuilder
            .HasOne(b => b.Country)
            .WithMany(c => c.Brands)
            .HasForeignKey(b => b.CountryId)
            .HasConstraintName("FK__brands__countries__country_id")
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder
            .HasIndex(b => b.Name)
            .IsUnique()
            .HasDatabaseName("IX__brands__name");
    }
    #endregion
}
