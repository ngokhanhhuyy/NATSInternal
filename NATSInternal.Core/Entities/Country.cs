namespace NATSInternal.Core.Entities;

[Table("countries")]
internal class Country : IHasIdEntity<Country>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("name")]
    [Required]
    [StringLength(CountryContracts.NameMaxLength)]
    public required string Name { get; set; }

    [Column("code")]
    [Required]
    [StringLength(CountryContracts.CodeMaxLength)]
    public required string Code { get; set; }
    #endregion

    #region NavigationProperties
    public List<Brand> Brands { get; private set; } = new();
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Country> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder
            .HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("IX__countries__name");
        entityBuilder
            .HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX__countries__code");
    }
    #endregion
}