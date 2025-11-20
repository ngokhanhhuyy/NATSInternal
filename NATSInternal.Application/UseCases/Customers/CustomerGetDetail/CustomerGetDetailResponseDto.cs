using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

public record CustomerGetDetailResponseDto
{
    #region Constructors
    internal CustomerGetDetailResponseDto(
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
        CreatedUserId = customer.CreatedUserId;
        CreatedDateTime = customer.CreatedDateTime;
        LastUpdatedUserId = customer.LastUpdatedUserId;
        LastUpdatedDateTime = customer.LastUpdatedDateTime;
        Authorization = authorizationResponseDto;

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
    public Guid CreatedUserId { get; }
    public DateTime CreatedDateTime { get; }
    public Guid? LastUpdatedUserId { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public CustomerBasicResponseDto? Introducer { get; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}
