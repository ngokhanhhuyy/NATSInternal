using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Supplies;

namespace NATSInternal.Core.Persistence.DbContext;

internal class PhotoEntityConfiguration : IEntityTypeConfiguration<Photo>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Photo> entityBuilder)
    {
        // Relationships.
        entityBuilder
            .HasOne<Product>()
            .WithMany(p => p.Photos)
            .HasForeignKey(photo => photo.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        entityBuilder
            .HasOne<Supply>()
            .WithMany(s => s.Photos)
            .HasForeignKey(photo => photo.SupplyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    #endregion
}