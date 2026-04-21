using Bogus.Extensions;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Customers;

internal class Customer
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    [StringLength(CustomerContracts.FirstNameMaxLength)]
    public required string FirstName
    {
        get => field;
        set
        {
            field = value;
            FullName = ComputeFullName();
        }
    }

    [StringLength(CustomerContracts.MiddleNameMaxLength)]
    public required string? MiddleName
    {
        get => field;
        set
        {
            field = value;
            FullName = ComputeFullName();
        }
    }
    
    [Required]
    [StringLength(CustomerContracts.LastNameMaxLength)]
    public required string LastName
    {
        get => field;
        set
        {
            field = value;
            FullName = ComputeFullName();
        }
    }

    [Required]
    [StringLength(CustomerContracts.FullNameMaxLength)]
    public string FullName
    {
        get => field;
        private set
        {
            field = value;
            NormalizedFullName = FullName.RemoveDiacritics();
        }
    } = string.Empty;
    
    [Required]
    [StringLength(CustomerContracts.FullNameMaxLength)]
    public string NormalizedFullName { get; private set; } = string.Empty;
    
    [StringLength(CustomerContracts.NickNameMaxLength)]
    public required string? NickName { get; set; }
    
    [Required]
    public required Gender Gender { get; set; }
    
    [Required]
    public required DateOnly? Birthday { get; set; }
    
    [StringLength(CustomerContracts.PhoneNumberMaxLength)]
    public required string? PhoneNumber { get; set; }
    
    [StringLength(CustomerContracts.ZaloNumberMaxLength)]
    public required string? ZaloNumber { get; set; }
    
    [StringLength(CustomerContracts.FacebookUrlMaxLength)]
    public required string? FacebookUrl { get; set; }
    
    [StringLength(CustomerContracts.EmailMaxLength)]
    public required string? Email { get; set; }
    
    [StringLength(CustomerContracts.AddressMaxLength)]
    public required string? Address { get; set; }
    
    [StringLength(CustomerContracts.NoteMaxLength)]
    public required string? Note { get; set; }
    
    [Required]
    public required DateTime CreatedDateTime { get; set; }
    
    public DateTime? LastUpdatedDateTime { get; set; }
    
    public DateTime? DeletedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    public int? IntroducerId { get; set; }
    
    [Required]
    public required int CreatedUserId { get; set; }
    public int? LastUpdatedUserId { get; set; }
    public int? DeletedUserId { get; set; }
    #endregion

    #region NavigationProperties
    public Customer? Introducer { get; set; }
    public List<Customer> IntroducedCustomers { get; set; } = new();

    public User CreatedUser { get; set; } = null!;
    public User? LastUpdatedUser { get; set; }
    public User? DeletedUser { get; set; }
    #endregion

    #region PrivateMethods
    private string ComputeFullName()
    {
        string?[] names = new[] { FirstName, MiddleName, LastName };
        return string.Join(" ", names.Where(n => n is not null));
    }
    #endregion
}