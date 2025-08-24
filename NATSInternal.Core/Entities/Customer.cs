using Bogus.Extensions;

namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(CustomerEntityConfiguration))]
[Table("customers")]
internal class Customer : AbstractEntity<Customer>, IUpsertableEntity<Customer>
{
    #region Fields
    private string _firstName = string.Empty;
    private string? _middleName;
    private string _lastName = string.Empty;
    private User? _createdUser;
    private Customer? _introducer;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("first_name")]
    [BackingField(nameof(_firstName))]
    [Required]
    [StringLength(CustomerContracts.FirstNameMaxLength)]
    public required string FirstName
    {
        get => _firstName;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedFirstName = value.RemoveDiacritics().ToUpper();
            _firstName = value;
        }
    }

    [Column("normalized_first_name")]
    [Required]
    [StringLength(CustomerContracts.FirstNameMaxLength)]
    public string NormalizedFirstName { get; private set; } = string.Empty;

    [Column("middle_name")]
    [BackingField(nameof(_middleName))]
    [StringLength(CustomerContracts.MiddleNameMaxLength)]
    public string? MiddleName
    {
        get => _middleName;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedMiddleName = value.RemoveDiacritics().ToUpper();
            _middleName = value;
        }
    }

    [Column("normalized_middle_name")]
    [StringLength(CustomerContracts.MiddleNameMaxLength)]
    public string? NormalizedMiddleName { get; private set; }

    [Column("last_name")]
    [BackingField(nameof(_lastName))]
    [Required]
    [StringLength(CustomerContracts.LastNameMaxLength)]
    public required string LastName
    {
        get => _lastName;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedLastName = value.RemoveDiacritics().ToUpper();
            _lastName = value;
        }
    }

    [Column("normalized_last_name")]
    [Required]
    [StringLength(CustomerContracts.LastNameMaxLength)]
    public string NormalizedLastName { get; set; } = string.Empty;

    [Column("full_name")]
    [Required]
    [StringLength(CustomerContracts.FullNameMaxLength)]
    public string FullName { get; private set; } = string.Empty;

    [Column("normalized_full_name")]
    [Required]
    [StringLength(CustomerContracts.FullNameMaxLength)]
    public string NormalizedFullName { get; private set; } = string.Empty;

    [Column("nick_name")]
    [StringLength(CustomerContracts.NickNameMaxLength)]
    public string? NickName { get; set; }

    [Column("gender")]
    [Required]
    public Gender Gender { get; set; }

    [Column("birthday")]
    public DateOnly? Birthday { get; set; }

    [Column("phone_number")]
    [StringLength(CustomerContracts.PhoneNumberMaxLength)]
    public string? PhoneNumber { get; set; }

    [Column("zalo_number")]
    [StringLength(CustomerContracts.ZaloNumberMaxLength)]
    public string? ZaloNumber { get; set; }

    [Column("facebook_url")]
    [StringLength(CustomerContracts.FacebookUrlMaxLength)]
    public string? FacebookUrl { get; set; }

    [Column("email")]
    [StringLength(CustomerContracts.EmailMaxLength)]
    public string? Email { get; set; }

    [Column("address")]
    [StringLength(CustomerContracts.AddressMaxLength)]
    public string? Address { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("last_updated_datetime")]
    public DateTime? LastUpdatedDateTime { get; set; }

    [Column("note")]
    [StringLength(CustomerContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region CachedProperties
    [Column("cached_incurred_debt_amount")]
    [Required]
    public long CachedIncurredDebtAmount { get; set; } = 0;

    [Column("cached_paid_debt_amount")]
    [Required]
    public long CachedPaidDebtAmount { get; set; } = 0;
    #endregion

    #region ForeignKeyProperties
    [Column("introducer_id")]
    public Guid? IntroducerId { get; set; }

    [Column("created_user_id")]
    [Required]
    public Guid CreatedUserId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingField
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_createdUser))]
    public User CreatedUser
    {
        get => GetFieldOrThrowIfNull(_createdUser);
        set => _createdUser = value;
    }

    [BackingField(nameof(_introducer))]
    public Customer? Introducer
    {
        get => _introducer;
        set
        {
            IntroducerId = value?.Id;
            _introducer = value;
        }
    }

    public List<Customer> IntroducedCustomers { get; protected set; } = new();
    public List<Order> Orders { get; protected set; } = new();
    public List<Debt> Debts { get; protected set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public IEnumerable<Debt> DebtIncurrences => Debts.Where(d => d.Type == DebtType.Incurrence);

    [NotMapped]
    public IEnumerable<Debt> DebtPayments => Debts.Where(d => d.Type == DebtType.Payment);

    [NotMapped]
    public long DebtIncurredAmount => DebtIncurrences.Sum(d => d.Amount);

    [NotMapped]
    public long DebtPaidAmount => DebtPayments.Sum(d => d.Amount);

    [NotMapped]
    public long RemainingDebtAmount => DebtIncurredAmount - DebtPaidAmount;

    [NotMapped]
    public long CachedDebtAmount => CachedIncurredDebtAmount - CachedPaidDebtAmount;
    #endregion

    #region PrivateMethods
    private void ComputeFullNameAndNormalizedFullName()
    {
        string?[] names = new[] { FirstName, MiddleName, LastName };
        FullName = string.Join(" ", names.Where(n => n is not null));
        NormalizedFullName = FullName.RemoveDiacritics().ToUpper();
    }
    #endregion
}