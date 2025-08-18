namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(CountryEntityConfigration))]
[Table("countries")]
internal class Country : AbstractEntity<Country>, IHasIdEntity<Country>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

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
    public List<Brand> Brands { get; protected set; } = new();
    #endregion
}