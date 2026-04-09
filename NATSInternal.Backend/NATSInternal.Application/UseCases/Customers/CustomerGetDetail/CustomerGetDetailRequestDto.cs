using MediatR;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerGetDetailRequestDto : IRequest<CustomerGetDetailResponseDto>
{
    #region Properties
    public required Guid Id { get; init; }
    #endregion
}
