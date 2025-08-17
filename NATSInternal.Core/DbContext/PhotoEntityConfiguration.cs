namespace NATSInternal.Core.DbContext;

internal class PhotoEntityConfiguration
{
    #region Methods
    public void Configure(EntityTypeBuilder<Photo> entityBuilder)
    {
        entityBuilder.HasKey(op => op.Id);
        entityBuilder
            .HasOne(p => p.Product)
            .WithMany
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