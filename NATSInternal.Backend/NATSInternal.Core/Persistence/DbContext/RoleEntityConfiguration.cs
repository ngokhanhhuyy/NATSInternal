using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Persistence.DbContext;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Role> entityBuilder)
    {
        // Indexes.
        entityBuilder.HasIndex(r => r.Name).IsUnique();
    }
    #endregion
}