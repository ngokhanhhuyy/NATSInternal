using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Supplies;

namespace NATSInternal.Infrastructure.DbContext.Supplies;

internal class SupplyItemEntityConfiguration : IEntityTypeConfiguration<SupplyItem>
{
    #region Methods
    public void Configure(EntityTypeBuilder<SupplyItem> builder)
    {
        // Primary key.
        builder.HasKey(si => si.Id);
        
        // Relationships.
        builder.HasOne<Supply>()
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SupplyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Properties.
        builder.Property(si => si.AmountPerUnit).IsRequired();
        builder.Property(si => si.Quantity).IsRequired();
        builder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}