using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Customers;

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
        }
    }

    internal CustomerBasicResponseDto(
        Customer? customer,
        CustomerExistingAuthorizationResponseDto authorization) : this(customer)
    {
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string FullName { get; } = string.Empty;
    public string? NickName { get; }
    public bool IsDeleted { get; } = true;
    public CustomerExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}