using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Persistence.DbContext;

internal class ProductCategoryEntityConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    #region Methods
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasIndex(pc => pc.Name).IsUnique();
    }
    #endregion
}
