using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Customers;

public record CustomerGetDetailResponseDto
{
    #region Constructors
    internal CustomerGetDetailResponseDto(
        Customer customer,
        User? createdUser,
        User? lastUpdatedUser,
        long debtRemainingAmount,
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
        CreatedUser = new(createdUser);
        CreatedDateTime = customer.CreatedDateTime;
        LastUpdatedDateTime = customer.LastUpdatedDateTime;
        DebtRemainingAmount = debtRemainingAmount;
        Authorization = authorizationResponseDto;

        if (lastUpdatedUser is not null)
        {
            LastUpdatedUser = new(lastUpdatedUser);
        }

        if (customer.Introducer is not null)
        {
            Introducer = new(customer.Introducer);
        }
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
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
    public long DebtRemainingAmount { get; }
    public CustomerBasicResponseDto? Introducer { get; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}
