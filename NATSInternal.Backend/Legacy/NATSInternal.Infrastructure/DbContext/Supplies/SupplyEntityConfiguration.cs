using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Supplies;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext.Supplies;

internal class SupplyEntityConfiguration : IEntityTypeConfiguration<Supply>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Supply> builder)
    {
        // Primary key.
        builder.HasKey(s => s.Id);

        // Relationships.
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Properties.
        builder.Property(s => s.ShipmentFee).IsRequired();
        builder.Property(s => s.BillCode).HasMaxLength(SupplyContracts.BillCodeMaxLength);
        builder.Property(s => s.TransactionDateTime).IsRequired();
        builder.Property(s => s.CreatedDateTime).IsRequired();
        builder.Property<long>("CachedItemAmount").IsRequired();
        builder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}