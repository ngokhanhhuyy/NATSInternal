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
    #endregion
    
    #region Properties
    public int Id { get; }
    public string FullName { get; } = string.Empty;
    public string? NickName { get; }
    public bool IsDeleted { get; } = true;
    #endregion
}