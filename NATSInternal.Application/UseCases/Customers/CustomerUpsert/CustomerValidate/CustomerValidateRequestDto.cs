using MediatR;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerValidateRequestDto : IRequest
{
    #region Properties
    public required CustomerUpsertRequestDto Data { get; set; }
    #endregion
}