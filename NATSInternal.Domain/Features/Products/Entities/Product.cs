using System.Diagnostics.CodeAnalysis;
using NATSInternal.Domain.Extensions;
using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

internal class Product : AbstractEntity
{
    #region Fields
    private Brand? _brand;
    private ProductCategory? _category;
    #endregion

    #region Constructors
#nullable disable
    private Product() { }
#nullable enable

    public Product(
        string name,
        string? description,
        string unit,
        long defaultAmountBeforeVatPerUnit,
        int defaultVatPercentage,
        bool isForRetail,
        bool isDiscontinued,
        DateTime createdDateTime,
        Brand? brand,
        ProductCategory? category)
    {
        PopulateProperties(
            name,
            description,
            unit,
            defaultAmountBeforeVatPerUnit,
            defaultVatPercentage,
            isForRetail,
            isDiscontinued,
            brand,
            category
        );
        CreatedDateTime = createdDateTime;
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public string? Description { get; private set; }
    public string Unit { get; private set; }
    public long DefaultAmountBeforeVatPerUnit { get; private set; }
    public int DefaultVatPercentage { get; private set; }
    public bool IsForRetail { get; private set; } = true;
    public bool IsDiscontinued { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime? LastUpdatedDateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    #endregion

    #region ForeignKeyProperties
    public Guid? BrandId { get; private set; }
    public Guid? CategoryId { get; private set; }
    #endregion

    #region NavigationProperties
    public Brand? Brand
    {
        get => _brand;
        private set
        {
            BrandId = value?.Id;
            _brand = value;
        }
    }

    public ProductCategory? Category
    {
        get => _category;
        private set
        {
            CategoryId = value?.Id;
            _category = value;
        }
    }
    #endregion

    #region Methods
    public void Update(
        string name,
        string? description,
        string unit,
        long defaultAmountBeforeVatPerUnit,
        int defaultVatPercentage,
        bool isForRetail,
        bool isDiscontinued,
        DateTime updatedDateTime,
        Brand? brand,
        ProductCategory? category)
    {
        PopulateProperties(
            name,
            description,
            unit,
            defaultAmountBeforeVatPerUnit,
            defaultVatPercentage,
            isForRetail,
            isDiscontinued,
            brand,
            category
        );
        LastUpdatedDateTime = updatedDateTime;
    }
    #endregion

    #region PrivateMethods
    [MemberNotNull(nameof(Name))]
    [MemberNotNull(nameof(NormalizedName))]
    [MemberNotNull(nameof(Unit))]
    private void PopulateProperties(
        string name,
        string? description,
        string unit,
        long defaultAmountBeforeVatPerUnit,
        int defaultVatPercentage,
        bool isForRetail,
        bool isDiscontinued,
        Brand? brand,
        ProductCategory? category)
    {
        Name = name;
        NormalizedName = name.ToUpper().ToNonDiacritics();
        Description = description;
        Unit = unit;
        DefaultAmountBeforeVatPerUnit = defaultAmountBeforeVatPerUnit;
        DefaultVatPercentage = defaultVatPercentage;
        IsForRetail = isForRetail;
        IsDiscontinued = isDiscontinued;
        Brand = brand;
        Category = category;
    }
    #endregion
}