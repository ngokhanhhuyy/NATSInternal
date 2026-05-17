using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Persistence.DbContext;

internal class SeedEntityConfiguration : IEntityTypeConfiguration<Seed>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Seed> entityBuilder)
    {
    }
    #endregion
}


internal class Seed
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public bool IsSeeded { get; set; }
    #endregion
}
