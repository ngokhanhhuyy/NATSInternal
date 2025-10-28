using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class ProductUpdateRequestDto : AbstractProductUpsertRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
}