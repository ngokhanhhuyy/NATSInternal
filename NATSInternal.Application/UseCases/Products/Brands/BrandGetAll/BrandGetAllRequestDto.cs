using MediatR;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public class BrandGetAllRequestDto : IRequest<IEnumerable<BrandBasicResponseDto>>, IRequestDto
{
    #region Methods
    public void TransformValues()
    {
    }
    #endregion
}