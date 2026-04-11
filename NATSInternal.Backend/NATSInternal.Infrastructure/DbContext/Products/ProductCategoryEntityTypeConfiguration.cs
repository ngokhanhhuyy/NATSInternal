using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext;

internal class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    #region Methods
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        // Primary key.
        builder.HasKey(pc => pc.Id);

        // Relationships.
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(pc => pc.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(pc => pc.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Properties.
        builder.Property(pc => pc.Name).HasMaxLength(ProductCategoryContracts.NameMaxLength).IsRequired();
        builder.Property(pc => pc.CreatedDateTime).IsRequired();
    }
    #endregion
}