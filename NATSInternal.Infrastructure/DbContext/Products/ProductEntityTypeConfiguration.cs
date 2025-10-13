using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext;

internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Primary key.
        builder.HasKey(p => p.Id);
        
        // Relationships.
        builder
            .HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

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
        
        // Properties.
        builder.Property(p => p.Name).HasMaxLength(ProductContracts.NameMaxLength).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(ProductContracts.DescriptionMaxLength);
        builder.Property(p => p.Unit).HasMaxLength(ProductContracts.UnitMaxLength).IsRequired();
        builder.Property(p => p.DefaultAmountBeforeVatPerUnit).IsRequired();
        builder.Property(p => p.DefaultVatPercentage).IsRequired();
        builder.Property(p => p.IsForRetail).IsRequired();
        builder.Property(p => p.IsDiscontinued).IsRequired();
        builder.Property(p => p.CreatedDateTime).IsRequired();
        
        // Indexes.
        builder.HasIndex(p => p.Name).IsUnique();
        
        // Query filters.
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
    #endregion
}