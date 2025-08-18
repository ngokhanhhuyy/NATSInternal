namespace NATSInternal.Core.DbContext;

internal class UpdateHistoryEntityConfiguration : IEntityTypeConfiguration<UpdateHistory>
{
    #region Methods
    public void Configure(EntityTypeBuilder<UpdateHistory> entityBuilder)
    {
        entityBuilder.HasKey(uh => uh.Id);
        entityBuilder
            .HasOne(uh => uh.Supply)
            .WithMany(s => s.UpdateHistories)
            .HasForeignKey(uh => uh.SupplyId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasOne(uh => uh.Expense)
            .WithMany(e => e.UpdateHistories)
            .HasForeignKey(uh => uh.SupplyId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasOne(uh => uh.Order)
            .WithMany(o => o.UpdateHistories)
            .HasForeignKey(uh => uh.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder
            .HasOne(uh => uh.UpdatedUser)
            .WithMany(u => u.OrderUpdateHistories)
            .HasForeignKey(uh => uh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(uh => uh.UpdatedDateTime);
        entityBuilder.Property(uh => uh.SerializedOldData).HasColumnType("JSON");
        entityBuilder.Property(uh => uh.SerializedNewData).HasColumnType("JSON");
    }
    #endregion
}
