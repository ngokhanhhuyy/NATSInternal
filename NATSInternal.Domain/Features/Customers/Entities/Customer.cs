using JetBrains.Annotations;
using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Customers;

internal class Customer : AbstractAggregateRootEntity
{
    #region Fields
    private Customer? _introducer;
    #endregion
    
    #region Constructors
    #nullable disable
    [UsedImplicitly]
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
        FullName = ComputeFullName();
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

        CustomerSnapshot snapshot = new(this);
        AddDomainEvent(new CustomerCreatedEvent(snapshot, createdDateTime));
    }
    #endregion

    #region Properties
    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; }
    public string FullName { get; private set; }
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
    #endregion

    #region ForeignKeyProperties
    public Guid? IntroducerId { get; private set; }
    public Guid CreatedUserId { get; private set; }
    public Guid? LastUpdatedUserId { get; private set; }
    public Guid? DeletedUserId { get; private set; }
    #endregion

    #region NavigationProperties
    public Customer? Introducer
    {
        get => _introducer;
        private set
        {
            IntroducerId = value?.Id;
            _introducer = value;
        }
    }

    public List<Customer> IntroducedCustomers { get; protected set; } = new();
    #endregion

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
        DateTime updatedDateTime,
        Guid updatedUserId)
    {
        CustomerSnapshot beforeUpdatingSnapshot = new(this);
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        FullName = ComputeFullName();
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
        LastUpdatedDateTime = updatedDateTime;
        LastUpdatedUserId = updatedUserId;
        CustomerSnapshot afterUpdatingSnapshot = new(this);

        AddDomainEvent(new CustomerUpdatedEvent(beforeUpdatingSnapshot, afterUpdatingSnapshot, updatedDateTime));
    }

    public void Delete(DateTime deletedDateTime, Guid deletedUserId)
    {
        CustomerSnapshot beforeDeletingSnapshot = new(this);
        DeletedDateTime = deletedDateTime;
        DeletedUserId = deletedUserId;

        AddDomainEvent(new CustomerDeletedEvent(beforeDeletingSnapshot, deletedDateTime));
    }
    #endregion

    #region PrivateMethods
    private string ComputeFullName()
    {
        string?[] names = new[] { FirstName, MiddleName, LastName };
        return string.Join(" ", names.Where(n => n is not null));
    }
    #endregion
}