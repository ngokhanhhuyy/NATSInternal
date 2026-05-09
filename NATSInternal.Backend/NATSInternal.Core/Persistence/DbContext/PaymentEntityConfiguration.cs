using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Payments;

namespace NATSInternal.Core.Persistence.DbContext;

internal class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Payment> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne(p => p.Order)
            .WithOne(p => p.Payment)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        entityBuilder
            .HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entityBuilder
            .HasOne(p => p.CreatedUser)
            .WithMany()
            .HasForeignKey(p => p.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder.HasOne(p => p.LastUpdatedUser)
            .WithMany()
            .HasForeignKey(p => p.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(p => p.DeletedUser)
            .WithMany()
            .HasForeignKey(p => p.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes.
        entityBuilder.HasIndex(p => p.StatsDate);
        entityBuilder.HasIndex(p => p.CreatedDateTime);
        entityBuilder.HasIndex(p => p.LastUpdatedDateTime);

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}