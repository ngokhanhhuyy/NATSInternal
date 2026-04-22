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
        }
    }
    #endregion
    
    #region Properties
    public Guid Id { get; } = Guid.Empty;
    public string FullName { get; } = string.Empty;
    public string? NickName { get; }
    public bool IsDeleted { get; } = true;
    #endregion
}