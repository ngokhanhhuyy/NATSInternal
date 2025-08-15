namespace NATSInternal.Core.Entities;

internal class Brand : IHasSinglePhotoEntity<Brand>
{
    #region Fields
    private Country? _country;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(BrandContracts.NameMaxLength)]
    public required string Name { get; set; }

    [StringLength(BrandContracts.WebsiteMaxLength)]
    public string? Website { get; set; }

    [StringLength(BrandContracts.SocialMediaUrlMaxLength)]
    public string? SocialMediaUrl { get; set; }

    [StringLength(BrandContracts.PhoneNumberMaxLength)]
    public string? PhoneNumber { get; set; }

    [StringLength(BrandContracts.EmailMaxLength)]
    public string? Email { get; set; }

    [StringLength(BrandContracts.AddressMaxLength)]
    public string? Address { get; set; }

    [StringLength(BrandContracts.ThumbnailUrlMaxLength)]
    public string? ThumbnailUrl { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    public int? CountryId { get; set; }
    #endregion

    #region NavigationProperties
    public List<Product> Products { get; private set; } = new();

    [BackingField(nameof(_country))]
    public Country Country
    {
        get => _country ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(Country)));
        set => _country = value;
    }
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Brand> entityBuilder)
    {
        entityBuilder.HasKey(b => b.Id);
        entityBuilder.HasOne(b => b.Country)
            .WithMany(c => c.Brands)
            .HasForeignKey(b => b.CountryId)
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder.HasIndex(b => b.Name).IsUnique();
    }
    #endregion
}