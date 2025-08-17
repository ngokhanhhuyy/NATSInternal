namespace NATSInternal.Core.Entities;

[Table("brands")]
internal class Brand : AbstractEntity<Brand>, IHasSinglePhotoEntity<Brand>
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

    [Column("thumbnail_url")]
    [StringLength(BrandContracts.ThumbnailUrlMaxLength)]
    public string? ThumbnailUrl { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("country_id")]
    public Guid? CountryId { get; set; }
    #endregion

    #region NavigationProperties
    public List<Product> Products { get; private set; } = new();

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

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Brand> entityBuilder)
    {
        entityBuilder.HasKey(b => b.Id);
        entityBuilder
            .HasOne(b => b.Country)
            .WithMany(c => c.Brands)
            .HasForeignKey(b => b.CountryId)
            .HasConstraintName("FK__brands__countries__country_id")
            .OnDelete(DeleteBehavior.SetNull);
        entityBuilder
            .HasIndex(b => b.Name)
            .IsUnique()
            .HasDatabaseName("IX__brands__name");
    }
    #endregion
}