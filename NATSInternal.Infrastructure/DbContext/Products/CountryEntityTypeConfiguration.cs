using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Products;

internal class CountryEntityTypeConfiguration : IEntityTypeConfiguration<Country>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        // Primary key.
        builder.HasKey(r => r.Id);

        // Index.
        builder.HasIndex(r => r.Name);

        // Properties.
        builder.Property(r => r.Name).HasMaxLength(CountryContracts.NameMaxLength).IsRequired();
    }
    #endregion
}