using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext.Customers;

internal class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Primary key.
        builder.HasKey(c => c.Id);

        // Relationships.
        builder.HasOne(c => c.Introducer).WithMany();
        builder.HasOne<User>().WithMany().HasForeignKey(c => c.CreatedUserId).IsRequired();
        builder.HasOne<User>().WithMany().HasForeignKey(c => c.LastUpdatedUserId);
        builder.HasOne<User>().WithMany().HasForeignKey(c => c.DeletedUserId);

        // Properties.
        builder.Property(c => c.FirstName).HasMaxLength(CustomerContracts.FirstNameMaxLength).IsRequired();
        builder.Property(c => c.MiddleName).HasMaxLength(CustomerContracts.MiddleNameMaxLength);
        builder.Property(c => c.LastName).HasMaxLength(CustomerContracts.LastNameMaxLength).IsRequired();
        builder.Property(c => c.FullName).HasMaxLength(CustomerContracts.FullNameMaxLength).IsRequired();
        builder.Property(c => c.NickName).HasMaxLength(CustomerContracts.NickNameMaxLength);
        builder.Property(c => c.PhoneNumber).HasMaxLength(CustomerContracts.PhoneNumberMaxLength);
        builder.Property(c => c.ZaloNumber).HasMaxLength(CustomerContracts.ZaloNumberMaxLength);
        builder.Property(c => c.FacebookUrl).HasMaxLength(CustomerContracts.FacebookUrlMaxLength);
        builder.Property(c => c.Email).HasMaxLength(CustomerContracts.EmailMaxLength);
        builder.Property(c => c.Address).HasMaxLength(CustomerContracts.AddressMaxLength);
        builder.Property(c => c.CreatedDateTime).IsRequired();
        builder.Property(c => c.Note).HasMaxLength(CustomerContracts.NoteMaxLength);
        builder.Property<long>("CachedDebtRemainingAmount").IsRequired();
        builder.Property<byte[]?>("RowVersion").IsRowVersion();

        // Indexes.
        builder.HasIndex(c => c.DeletedDateTime);
    }
    #endregion
}