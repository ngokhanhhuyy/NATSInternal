using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class ProductUpdateRequestDto : AbstractProductUpsertRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    public bool IsDiscontinued { get; set; }
    #endregion
}