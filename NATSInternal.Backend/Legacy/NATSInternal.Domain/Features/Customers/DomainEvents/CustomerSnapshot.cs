namespace NATSInternal.Domain.Features.Customers;

public class CustomerSnapshot
{
    #region Constructors
    internal CustomerSnapshot(Customer customer)
    {
        FirstName = customer.FirstName;
        MiddleName = customer.MiddleName;
        LastName = customer.LastName;
        NickName = customer.LastName;
        Gender = customer.Gender;
        Birthday = customer.Birthday;
        PhoneNumber = customer.PhoneNumber;
        ZaloNumber = customer.ZaloNumber;
        FacebookUrl = customer.ZaloNumber;
        Email = customer.Email;
        Address = customer.Address;
        Note = customer.Note;
        IntroducerId = customer.IntroducerId;
    }
    #endregion

    #region Properties
    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName { get; }
    public string? MiddleName { get; }
    public string LastName { get; }
    public string FullName { get; } = string.Empty;
    public string? NickName { get; }
    public Gender Gender { get; }
    public DateOnly? Birthday { get; }
    public string? PhoneNumber { get; }
    public string? ZaloNumber { get; }
    public string? FacebookUrl { get; }
    public string? Email { get; }
    public string? Address { get; }
    public string? Note { get; }
    public Guid? IntroducerId { get; }
    #endregion
}