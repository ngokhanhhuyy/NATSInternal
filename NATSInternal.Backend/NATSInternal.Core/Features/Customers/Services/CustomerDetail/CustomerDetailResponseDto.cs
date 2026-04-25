using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Customers;

public class CustomerDetailResponseDto
{
    #region Constructors
    internal CustomerDetailResponseDto(
        Customer customer,
        CustomerExistingAuthorizationResponseDto authorizationResponseDto)
    {
        Id = customer.Id;
        FirstName = customer.FirstName;
        MiddleName = customer.MiddleName;
        LastName = customer.LastName;
        FullName = customer.FullName;
        NickName = customer.NickName;
        Gender = customer.Gender;
        Birthday = customer.Birthday;
        PhoneNumber = customer.PhoneNumber;
        ZaloNumber = customer.ZaloNumber;
        FacebookUrl = customer.FacebookUrl;
        Email = customer.Email;
        Address = customer.Address;
        Note = customer.Note;
        CreatedUser = new(customer.CreatedUser);
        CreatedDateTime = customer.CreatedDateTime;
        LastUpdatedDateTime = customer.LastUpdatedDateTime;
        DeletedDateTime = customer.DeletedDateTime;
        Authorization = authorizationResponseDto;

        if (customer.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(customer.LastUpdatedUser);
        }

        if (customer.Introducer is not null)
        {
            Introducer = new(customer.Introducer);
        }

        if (customer.DeletedUser is not null)
        {
            DeletedUser = new(customer.DeletedUser);
        }
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string FirstName { get; }
    public string? MiddleName { get; }
    public string LastName { get; }
    public string FullName { get; }
    public string? NickName { get; }
    public Gender Gender { get; }
    public DateOnly? Birthday { get; }
    public string? PhoneNumber { get; }
    public string? ZaloNumber { get; }
    public string? FacebookUrl { get; }
    public string? Email { get; }
    public string? Address { get; }
    public string? Note { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? DeletedUser { get; }
    public DateTime? DeletedDateTime { get; }
    public CustomerBasicResponseDto? Introducer { get; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}
