namespace NATSInternal.Core.DbContext;

internal class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Product> entityBuilder)
    {
        entityBuilder.HasKey(p => p.Id);
        entityBuilder
            .HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .HasConstraintName("FK__products__brands__brand_id")
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder
            .HasOne(p => p.Category)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("FK__products__product_categories__category_id")
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder
            .HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName("IX__products__name");
        entityBuilder
            .HasIndex()
            .HasDatabaseName("IX__products__is_deleted");
        entityBuilder.HasQueryFilter(p => !p.IsDeleted);
    }
    #endregion
}
