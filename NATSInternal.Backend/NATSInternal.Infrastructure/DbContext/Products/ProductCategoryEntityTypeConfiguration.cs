using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Infrastructure.DbContext;

internal class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    #region Methods
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        // Primary key.
        builder.HasKey(pc => pc.Id);
        
        // Properties.
        builder.Property(pc => pc.Name).HasMaxLength(ProductCategoryContracts.NameMaxLength).IsRequired();
    }
    #endregion
}