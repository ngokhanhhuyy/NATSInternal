using MediatR;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerCreateRequestDto : CustomerUpsertRequestDto, IRequest<Guid>;