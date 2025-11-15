using NATSInternal.Domain.Extensions;
using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Customers;

internal class Customer : AbstractEntity
{
    #region Constructors
    #nullable disable
    private Customer() { }
    #nullable restore

    public Customer(
        string firstName,
        string? middleName,
        string lastName,
        string? nickName,
        Gender gender,
        DateOnly birthday,
        string? phoneNumber,
        string? zaloNumber,
        string? facebookUrl,
        string? email,
        string? address,
        string? note,
        Customer? introducer,
        Guid createdUserId,
        DateTime createdDateTime)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        NickName = nickName;
        Gender = gender;
        Birthday = birthday;
        PhoneNumber = phoneNumber;
        ZaloNumber = zaloNumber;
        FacebookUrl = facebookUrl;
        Email = email;
        Address = address;
        Note = note;
        Introducer = introducer;
        CreatedUserId = createdUserId;
        CreatedDateTime = createdDateTime;
    }
    #endregion

    #region Properties
    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName
    {
        get;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedFirstName = value.ToNonDiacritics().ToUpper();
            field = value;
        }
    }

    public string NormalizedFirstName { get; private set; } = string.Empty;

    public string? MiddleName
    {
        get;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedMiddleName = value?.ToNonDiacritics().ToUpper();
            field = value;
        }
    }

    public string? NormalizedMiddleName { get; private set; }

    public string LastName
    {
        get;
        set
        {
            ComputeFullNameAndNormalizedFullName();
            NormalizedLastName = value.ToNonDiacritics().ToUpper();
            field = value;
        }
    }

    public string NormalizedLastName { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string NormalizedFullName { get; private set; } = string.Empty;
    public string? NickName { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly? Birthday { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? ZaloNumber { get; private set; }
    public string? FacebookUrl { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime? LastUpdatedDateTime { get; private set; }
    public DateTime? DeletedDateTime { get; private set; }
    public string? Note { get; private set; }
    public bool IsDeleted { get; private set; }
    #endregion

    // #region CachedProperties
    // public long CachedRemainingDebtAmount { get; set; }
    // #endregion

    #region ForeignKeyProperties
    public Guid? IntroducerId { get; private set; }
    public Guid CreatedUserId { get; private set; }
    #endregion

    // #region ConcurrencyOperationTrackingField
    // public byte[]? RowVersion { get; set; }
    // #endregion

    #region NavigationProperties
    public Customer? Introducer
    {
        get;
        private set
        {
            IntroducerId = value?.Id;
            field = value;
        }
    }

    public List<Customer> IntroducedCustomers { get; protected set; } = new();
    #endregion

    // #region ComputedProperties
    // [NotMapped]
    // public IEnumerable<Debt> DebtIncurrences => Debts.Where(d => d.Type == DebtType.Incurrence);

    // [NotMapped]
    // public IEnumerable<Debt> DebtPayments => Debts.Where(d => d.Type == DebtType.Payment);

    // [NotMapped]
    // public long DebtIncurredAmount => DebtIncurrences.Sum(d => d.Amount);

    // [NotMapped]
    // public long DebtPaidAmount => DebtPayments.Sum(d => d.Amount);

    // [NotMapped]
    // public long RemainingDebtAmount => DebtIncurredAmount - DebtPaidAmount;
    // #endregion

    #region Methods
    public void Update(
        string firstName,
        string? middleName,
        string lastName,
        string? nickName,
        Gender gender,
        DateOnly birthday,
        string? phoneNumber,
        string? zaloNumber,
        string? facebookUrl,
        string? email,
        string? address,
        string? note,
        Customer? introducer,
        DateTime updatedDateTime)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        NickName = nickName;
        Gender = gender;
        Birthday = birthday;
        PhoneNumber = phoneNumber;
        ZaloNumber = zaloNumber;
        FacebookUrl = facebookUrl;
        Email = email;
        Address = address;
        Note = note;
        Introducer = introducer;
        UpdatedDateTime = createdDateTime;
    }
    #endregion

    #region PrivateMethods
    private void ComputeFullNameAndNormalizedFullName()
    {
        string?[] names = new[] { FirstName, MiddleName, LastName };
        FullName = string.Join(" ", names.Where(n => n is not null));
        NormalizedFullName = FullName.ToNonDiacritics().ToUpper();
    }
    #endregion
}