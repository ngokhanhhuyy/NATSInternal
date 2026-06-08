using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Customers;

public class CustomerBasicResponseDto
{
    #region Constructors
    internal CustomerBasicResponseDto(Customer customer)
    {
        Id = customer.Id;
        FullName = customer.FullName;
        NickName = customer.NickName;
        DebtAmount = customer.CachedDebtAmount;
        IsDeleted = customer.DeletedDateTime is not null;
    }

    internal CustomerBasicResponseDto(
        Customer customer,
        CustomerExistingAuthorizationResponseDto authorization) : this(customer)
    {
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string FullName { get; }
    public string? NickName { get; }
    public long DebtAmount { get; set; }
    public bool IsDeleted { get; }
    public CustomerExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}
