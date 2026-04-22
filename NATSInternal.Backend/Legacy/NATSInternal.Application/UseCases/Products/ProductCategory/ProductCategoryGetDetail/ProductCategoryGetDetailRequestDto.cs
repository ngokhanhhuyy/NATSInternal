using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class ProductCategoryGetDetailRequestDto : IRequest<ProductCategoryGetDetailResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
}
