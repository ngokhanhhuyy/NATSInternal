using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Customers;

namespace NATSInternal.Core.Persistence.DbContext;

internal class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Customer> entityBuilder)
    {
        // Relationships.
        entityBuilder.HasOne(c => c.Introducer)
            .WithMany(i => i.IntroducedCustomers)
            .HasForeignKey(c => c.IntroducerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        entityBuilder.HasOne(c => c.CreatedUser)
            .WithMany()
            .HasForeignKey(u => u.CreatedUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(u => u.LastUpdatedUser)
            .WithMany()
            .HasForeignKey(u => u.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(u => u.DeletedUser)
            .WithMany()
            .HasForeignKey(u => u.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes.
        entityBuilder.HasIndex(c => c.NickName).IsUnique();

        // RowVersion.
        entityBuilder.Property<byte[]?>("RowVersion").IsRowVersion();
    }
    #endregion
}