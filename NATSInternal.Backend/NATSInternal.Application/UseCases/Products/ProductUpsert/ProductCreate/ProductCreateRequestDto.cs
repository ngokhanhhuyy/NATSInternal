using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class ProductCreateRequestDto : AbstractProductUpsertRequestDto, IRequest<Guid>;