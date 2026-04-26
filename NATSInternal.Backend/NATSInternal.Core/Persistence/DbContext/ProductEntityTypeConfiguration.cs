using Humanizer;
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
            .UsingEntity<ProductProductCategory>(
                joinerEntity => joinerEntity
                    .HasOne(ppc => ppc.Category)
                    .WithMany()
                    .HasForeignKey(j => j.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                joinerEntity => joinerEntity
                    .HasOne(ppc => ppc.Product)
                    .WithMany()
                    .HasForeignKey(ppc => ppc.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                joinerEntity =>
                {
                    joinerEntity.ToTable(nameof(ProductProductCategory).Pluralize());
                    joinerEntity.HasKey(ppc => new { ppc.ProductId, ppc.CategoryId });
                });
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

internal class ProductProductCategory
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