using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Shared;

public class CustomerBasicResponseDto
{
    #region Constructors
    internal CustomerBasicResponseDto(Customer? customer)
    {
        if (customer is not null && customer.DeletedDateTime is null)
        {
            Id = customer.Id;
            FullName = customer.FullName;
            NickName = customer.NickName;
            Gender = customer.Gender;
            Birthday = customer.Birthday;
            PhoneNumber = customer.PhoneNumber;
        }
        else
        {
            Id = Guid.Empty;
            FullName = string.Empty;
            NickName = string.Empty;
            Gender = Gender.Male;
            Birthday = DateOnly.MinValue;
            PhoneNumber = string.Empty;
            IsDeleted = true;
        }
    }

    internal CustomerBasicResponseDto(
        Customer customer,
        CustomerExistingAuthorizationResponseDto authorization) : this(customer)
    {
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string FullName { get; }
    public string? NickName { get; }
    public Gender Gender { get; }
    public DateOnly? Birthday { get; }
    public string? PhoneNumber { get; }
    public CustomerExistingAuthorizationResponseDto? Authorization { get; }
    public bool IsDeleted { get; }
    #endregion
}