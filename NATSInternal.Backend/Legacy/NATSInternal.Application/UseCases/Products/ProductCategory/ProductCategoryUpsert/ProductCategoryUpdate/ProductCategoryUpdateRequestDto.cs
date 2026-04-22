using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class ProductCategoryUpdateRequestDto : ProductCategoryUpsertRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
}