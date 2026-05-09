using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Expenses;

namespace NATSInternal.Core.Persistence.DbContext;

internal class ExpenseEntityConfiguration : IEntityTypeConfiguration<Expense>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Expense> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne(e => e.CreatedUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder.HasOne(e => e.LastUpdatedUser)
            .WithMany()
            .HasForeignKey(e => e.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(e => e.DeletedUser)
            .WithMany()
            .HasForeignKey(e => e.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes.
        entityBuilder.HasIndex(e => e.StatsDate);
        entityBuilder.HasIndex(e => e.CreatedDateTime);
        entityBuilder.HasIndex(e => e.LastUpdatedDateTime);

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}