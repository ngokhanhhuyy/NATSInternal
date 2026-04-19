namespace NATSInternal.Core.DbContext;

internal class ProductCategoryEntityConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    #region Methods
    public void Configure(EntityTypeBuilder<ProductCategory> entityBuilder)
    {
        entityBuilder.HasKey(pc => pc.Id);
        entityBuilder
            .HasIndex(pc => pc.Name)
            .IsUnique()
            .HasDatabaseName("IX__product_categories__name");
    }
    #endregion
}