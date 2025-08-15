namespace NATSInternal.Core.Entities;

internal class Country : IHasIdEntity<Country>
{
    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required]
    [StringLength(40)]
    public required string Name { get; set; }

    [Required]
    [StringLength(3)]
    public required string Code { get; set; }
    #endregion

    #region NavigationProperties
    public List<Brand> Brands { get; private set; } = new();
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Country> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.HasIndex(c => c.Name).IsUnique();
        entityBuilder.HasIndex(c => c.Code).IsUnique();
    }
    #endregion
}