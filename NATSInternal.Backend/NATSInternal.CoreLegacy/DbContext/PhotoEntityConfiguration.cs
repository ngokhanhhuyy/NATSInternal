namespace NATSInternal.Core.DbContext;

internal class PhotoEntityConfiguration
{
    #region Methods
    public void Configure(EntityTypeBuilder<Photo> entityBuilder)
    {
        entityBuilder.HasKey(op => op.Id);
        entityBuilder
            .HasOne(p => p.Brand)
            .WithMany(b => b.Photos)
            .HasForeignKey(p => p.BrandId)
            .HasConstraintName("FK__photos__brands__brand_id")
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasOne(photo => photo.Product)
            .WithMany(product => product.Photos)
            .HasForeignKey(photo => photo.ProductId)
            .HasConstraintName("FK__photos__products__photo_id")
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasOne(p => p.Supply)
            .WithMany(s => s.Photos)
            .HasForeignKey(p => p.SupplyId)
            .HasConstraintName("FK__photos__supplies__supply_id")
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasOne(p => p.Expense)
            .WithMany(e => e.Photos)
            .HasForeignKey(p => p.ExpenseId)
            .HasConstraintName("FK__photos__expenses__expense_id")
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasOne(p => p.Order)
            .WithMany(o => o.Photos)
            .HasForeignKey(p => p.OrderId)
            .HasConstraintName("FK__photos__orders__order_id")
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasIndex(p => p.Url)
            .IsUnique()
            .HasDatabaseName("IX__photos__url");
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}