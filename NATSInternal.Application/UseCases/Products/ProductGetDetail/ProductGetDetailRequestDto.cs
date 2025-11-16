using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetDetailRequestDto : IRequest<ProductGetDetailResponseDto>, IRequestDto
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
    
    #region Methods
    public void TransformValues() { }
    #endregion
}