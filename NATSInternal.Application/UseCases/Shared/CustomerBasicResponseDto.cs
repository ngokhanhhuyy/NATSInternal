using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Shared;

public class CustomerBasicResponseDto
{
    #region Constructors
    internal CustomerBasicResponseDto(Customer customer, long remainingDebtAmount)
    {
        Id = customer.Id;
        FullName = customer.FullName;
        NickName = customer.NickName;
        Gender = customer.Gender;
        Birthday = customer.Birthday;
        PhoneNumber = customer.PhoneNumber;
        RemainingDebtAmount = remainingDebtAmount;
    }

    internal CustomerBasicResponseDto(
        Customer customer,
        long remainingDebtAmount,
        CustomerExistingAuthorizationResponseDto authorization) : this(customer, remainingDebtAmount)
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
    public long? RemainingDebtAmount { get; }
    public CustomerExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}