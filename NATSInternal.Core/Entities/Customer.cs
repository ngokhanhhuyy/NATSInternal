using Bogus.Extensions;

namespace NATSInternal.Core.Entities;

internal class Customer : IUpsertableEntity<Customer>
{
    #region Fields
    private string _firstName = string.Empty;
    private string? _middleName;
    private string _lastName = string.Empty;
    private User? _createdUser;
    private Customer? _introducer;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [BackingField(nameof(_firstName))]
    [Required]
    [StringLength(CustomerContracts.FirstNameMaxLength)]
    public required string FirstName
    {
        get => _firstName;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedFirstName = value.RemoveDiacritics();
            _firstName = value;
        }
    }

    [Required]
    [StringLength(CustomerContracts.FirstNameMaxLength)]
    public string NormalizedFirstName { get; private set; } = string.Empty;

    [BackingField(nameof(_middleName))]
    [StringLength(CustomerContracts.MiddleNameMaxLength)]
    public string? MiddleName
    {
        get => _middleName;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedMiddleName = value.RemoveDiacritics();
            _middleName = value;
        }
    }

    [StringLength(CustomerContracts.MiddleNameMaxLength)]
    public string? NormalizedMiddleName { get; private set; }

    [BackingField(nameof(_lastName))]
    [Required]
    [StringLength(CustomerContracts.LastNameMaxLength)]
    public required string LastName
    {
        get => _lastName;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedLastName = value.RemoveDiacritics();
            _lastName = value;
        }
    }

    [Required]
    [StringLength(CustomerContracts.LastNameMaxLength)]
    public string NormalizedLastName { get; set; } = string.Empty;

    [Required]
    [StringLength(CustomerContracts.FullNameMaxLength)]
    public string FullName { get; private set; } = string.Empty;

    [Required]
    [StringLength(CustomerContracts.FullNameMaxLength)]
    public string NormalizedFullName { get; private set; } = string.Empty;

    [StringLength(CustomerContracts.NickNameMaxLength)]
    public string? NickName { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    [StringLength(CustomerContracts.PhoneNumberMaxLength)]
    public string? PhoneNumber { get; set; }

    [StringLength(CustomerContracts.ZaloNumberMaxLength)]
    public string? ZaloNumber { get; set; }

    [StringLength(CustomerContracts.FacebookUrlMaxLength)]
    public string? FacebookUrl { get; set; }

    [StringLength(CustomerContracts.EmailMaxLength)]
    public string? Email { get; set; }

    [StringLength(CustomerContracts.AddressMaxLength)]
    public string? Address { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    public DateTime? UpdatedDateTime { get; set; }

    [StringLength(CustomerContracts.NoteMaxLength)]
    public required string Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region CachedProperties
    [Required]
    public long CachedIncurredDebtAmount { get; set; } = 0;

    [Required]
    public long CachedPaidDebtAmount { get; set; } = 0;
    #endregion

    #region ForeignKeyProperties
    public Guid? IntroducerId { get; set; }

    [Required]
    public Guid CreatedUserId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingField
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_createdUser))]
    public User CreatedUser
    {
        get => _createdUser ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(CreatedUser)));
        set => _createdUser = value;
    }

    [BackingField(nameof(_introducer))]
    public Customer Introducer
    {
        get => _introducer ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(Introducer)));
        set => _introducer = value;
    }

    public List<Customer> IntroducedCustomers { get; private set; } = new();
    public List<Order> Orders { get; private set; } = new();
    public List<Treatment> Treatments { get; private set; } = new();
    public List<Consultant> Consultants { get; private set; } = new();
    public List<DebtIncurrence> DebtIncurrences { get; private set; } = new();
    public List<DebtPayment> DebtPayments { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public long DebtIncurredAmount => DebtIncurrences
        .Where(di => !di.IsDeleted)
        .Sum(di => di.Amount);

    [NotMapped]
    public long DebtPaidAmount => DebtPayments
        .Where(dp => !dp.IsDeleted)
        .Sum(dp => dp.Amount);

    [NotMapped]
    public long DebtAmount => DebtIncurredAmount - DebtPaidAmount;

    [NotMapped]
    public long CachedDebtAmount => CachedIncurredDebtAmount - CachedPaidDebtAmount;
    #endregion

    #region PrivateMethods
    private void ComputeFullNameAndNormalizedFullName()
    {
        string?[] names = new[] { FirstName, MiddleName, LastName };
        FullName = string.Join(" ", names.Where(n => n is not null));
        NormalizedFullName = FullName.RemoveDiacritics();
    }
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Customer> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.HasOne(c => c.Introducer)
            .WithMany(i => i.IntroducedCustomers)
            .HasForeignKey(c => c.IntroducerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(c => c.CreatedUser)
            .WithMany(u => u.CreatedCustomers)
            .HasForeignKey(c => c.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}