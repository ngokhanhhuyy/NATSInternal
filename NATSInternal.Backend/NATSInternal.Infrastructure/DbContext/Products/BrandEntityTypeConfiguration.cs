using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Infrastructure.DbContext;

internal class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        // Primary key.
        builder.HasKey(b => b.Id);

        // Relationship.
        builder
            .HasOne(b => b.Country)
            .WithMany()
            .HasForeignKey(b => b.CountryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Properties.
        builder.Property(b => b.Name).HasMaxLength(BrandContracts.NameMaxLength).IsRequired();
        builder.Property(b => b.Website).HasMaxLength(BrandContracts.WebsiteMaxLength);
        builder.Property(b => b.SocialMediaUrl).HasMaxLength(BrandContracts.SocialMediaUrlMaxLength);
        builder.Property(b => b.PhoneNumber).HasMaxLength(BrandContracts.PhoneNumberMaxLength);
        builder.Property(b => b.Email).HasMaxLength(BrandContracts.EmailMaxLength);
        builder.Property(b => b.Address).HasMaxLength(BrandContracts.AddressMaxLength);
        builder.Property(b => b.CreatedDateTime).IsRequired();

        // Index.
        builder.HasIndex(b => b.Name);
        builder.HasIndex(b => b.CreatedDateTime);
    }
    #endregion
}