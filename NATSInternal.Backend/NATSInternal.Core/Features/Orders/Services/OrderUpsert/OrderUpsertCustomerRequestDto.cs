using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Features.Customers;

namespace NATSInternal.Core.Features.Orders;

public class OrderUpsertCustomerRequestDto : IRequestDto
{
    #region Properties
    public int? Id { get; set; }
    public CustomerUpsertRequestDto? Create { get; set; }
    public bool CreateNewCustomer { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        Id = Id == 0 ? null : Id;
    }
    #endregion
}
