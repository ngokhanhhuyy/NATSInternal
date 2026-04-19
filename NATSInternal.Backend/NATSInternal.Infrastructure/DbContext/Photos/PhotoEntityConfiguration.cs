using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Supplies;

namespace NATSInternal.Infrastructure.DbContext.Photos;

internal class PhotoEntityConfiguration : IEntityTypeConfiguration<Photo>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        // Primary key.
        builder.HasKey(p => p.Id);
        
        // Relationships.
        builder.HasOne<Brand>()
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Supply>()
            .WithMany()
            .HasForeignKey(p => p.SupplyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Properties.
        builder.Property(p => p.Url).IsRequired();
        builder.Property(p => p.IsThumbnail).IsRequired();
    }
    
    #endregion
}