using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Products;

namespace NATSInternal.Infrastructure.DbContext.Photos;

internal class PhotoEntityConfiguration : IEntityTypeConfiguration<Photo>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Photo> entityBuilder)
    {
        // Relationships.
        entityBuilder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    #endregion
}