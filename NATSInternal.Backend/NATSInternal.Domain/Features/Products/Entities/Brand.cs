using JetBrains.Annotations;
using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

[UsedImplicitly]
internal class Brand : AbstractEntity
{
    #region Fields
    private Country? _country;
    #endregion

    #region Constructors
    #nullable disable
    private Brand() { }
    #nullable enable

    public Brand(
        string name,
        string? website,
        string? socialMediaUrl,
        string? phoneNumber,
        string? email,
        string? address,
        Guid createdUserId,
        DateTime createdDateTime,
        Country? country)
    {
        Name = name;
        Website = website;
        SocialMediaUrl = socialMediaUrl;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        CreatedUserId = createdUserId;
        CreatedDateTime = createdDateTime;
        Country = country;

        AddDomainEvent(new BrandCreatedEvent(Id, createdDateTime));
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string? Website { get; private set; }
    public string? SocialMediaUrl { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    public Guid CreatedUserId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public Guid? LastUpdatedUserId { get; private set; }
    public DateTime? LastUpdatedDateTime { get; private set; }
    public Guid? CountryId { get; private set; }
    #endregion

    #region NavigationProperties
    public Country? Country
    {
        get => _country;
        private set
        {
            CountryId = value?.Id;
            _country = value;
        }
    }
    #endregion

    #region Methods
    public void ApplyUpdate(
        string name,
        string? website,
        string? socialMediaUrl,
        string? phoneNumber,
        string? email,
        string? address,
        Guid updatedUserId,
        DateTime updatedDateTime,
        Country? country)
    {
        Name = name;
        Website = website;
        SocialMediaUrl = socialMediaUrl;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        LastUpdatedUserId = updatedUserId;
        LastUpdatedDateTime = updatedDateTime;
        Country = country;
        
        AddDomainEvent(new BrandUpdatedEvent(Id, updatedDateTime));
    }

    public void MarkAsDeleted(DateTime deletedDateTime)
    {
        AddDomainEvent(new BrandDeletedEvent(Id, deletedDateTime));
    }
    #endregion
}