namespace NATSInternal.Services.Entities;

internal class Country : IIdentifiableEntity<Country>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(40)]
    public string Name { get; set; }

    [Required]
    [StringLength(3)]
    public string Code { get; set; }

    // Relationships
    public virtual List<Brand> Brands { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Country> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.HasIndex(c => c.Name)
            .IsUnique();
        entityBuilder.HasIndex(c => c.Code)
            .IsUnique();
    }
}