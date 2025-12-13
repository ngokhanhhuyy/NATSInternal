using MediatR;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public class ProductCategoryGetAllRequestDto : IRequest<IEnumerable<ProductCategoryBasicResponseDto>>, IRequestDto
{
    #region Methods
    public void TransformValues()
    {
    }
    #endregion
}