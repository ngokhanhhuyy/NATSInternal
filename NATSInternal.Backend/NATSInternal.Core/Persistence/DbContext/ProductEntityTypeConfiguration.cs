using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Infrastructure.DbContext;

internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Relationships.
        builder
            .HasMany(p => p.Categories)
            .WithMany(pc => pc.Products)
            .UsingEntity<ProductAndProductCategoryJoiner>(
                joinerEntity => joinerEntity
                    .HasOne(j => j.Category)
                    .WithMany()
                    .HasForeignKey(j => j.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                joinerEntity => joinerEntity
                    .HasOne(j => j.Product)
                    .WithMany()
                    .HasForeignKey(j => j.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                joinerEntity => joinerEntity.HasKey(j => new { j.ProductId, j.CategoryId }));
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.LastUpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.DeletedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes.
        builder.HasIndex(p => p.Name).IsUnique();
    }
    #endregion
}

file class ProductAndProductCategoryJoiner
{
    #region Properties
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    #endregion

    #region NavigationProperties
    public Product Product { get; set; } = null!;
    public ProductCategory Category { get; set; } = null!;
    #endregion
}