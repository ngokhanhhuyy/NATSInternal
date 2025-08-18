namespace NATSInternal.Core.DbContext;

internal class SupplyItemEntityConfiguration : IEntityTypeConfiguration<SupplyItem>
{
    #region Methods
    public void Configure(EntityTypeBuilder<SupplyItem> entityBuilder)
    {
        entityBuilder.HasKey(si => si.Id);
        entityBuilder
            .HasOne(si => si.Supply)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SupplyId)
            .HasConstraintName("FK__supply_items__supplies__supply_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .HasOne(si => si.Product)
            .WithMany(p => p.SupplyItems)
            .HasForeignKey(si => si.ProductId)
            .HasConstraintName("FK__supply_items__products__product_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .Property(si => si.RowVersion)
            .IsRowVersion();
    }
    #endregion
}
