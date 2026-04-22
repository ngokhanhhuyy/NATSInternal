using MediatR;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerUpdateRequestDto : CustomerUpsertRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
}
