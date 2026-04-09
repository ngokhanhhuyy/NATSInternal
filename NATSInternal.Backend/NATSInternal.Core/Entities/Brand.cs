namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(BrandEntityConfiguration))]
[Table("brands")]
internal class Brand : AbstractEntity<Brand>, IHasThumbnailEntity<Brand>
{
    #region Fields
    private Country? _country;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("name")]
    [Required]
    [StringLength(BrandContracts.NameMaxLength)]
    public required string Name { get; set; }

    [Column("website")]
    [StringLength(BrandContracts.WebsiteMaxLength)]
    public string? Website { get; set; }

    [Column("social_media_url")]
    [StringLength(BrandContracts.SocialMediaUrlMaxLength)]
    public string? SocialMediaUrl { get; set; }

    [Column("phone_number")]
    [StringLength(BrandContracts.PhoneNumberMaxLength)]
    public string? PhoneNumber { get; set; }

    [Column("email")]
    [StringLength(BrandContracts.EmailMaxLength)]
    public string? Email { get; set; }

    [Column("address")]
    [StringLength(BrandContracts.AddressMaxLength)]
    public string? Address { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("country_id")]
    public Guid? CountryId { get; set; }
    #endregion

    #region NavigationProperties
    public List<Product> Products { get; protected set; } = new();
    public List<Photo> Photos { get; protected set; } = new();

    [BackingField(nameof(_country))]
    public Country? Country
    {
        get => _country;
        set
        {
            CountryId = value?.Id;
            _country = value;
        }
    }
    #endregion

    #region ComputedProperties
    [NotMapped]
    public string? ThumbnailUrl => Photos?
        .Where(p => p.IsThumbnail && p.BrandId == Id)
        .Select(p => p.Url)
        .SingleOrDefault();
    #endregion
}