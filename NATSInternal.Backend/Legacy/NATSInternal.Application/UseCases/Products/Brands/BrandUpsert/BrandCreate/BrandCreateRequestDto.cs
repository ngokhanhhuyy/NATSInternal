using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class BrandCreateRequestDto : BrandUpsertRequestDto, IRequest<Guid>;