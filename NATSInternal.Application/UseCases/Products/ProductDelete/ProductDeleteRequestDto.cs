using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class ProductDeleteRequestDto : IRequest, IRequestDto
{
    #region Properties
    public required Guid Id { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}