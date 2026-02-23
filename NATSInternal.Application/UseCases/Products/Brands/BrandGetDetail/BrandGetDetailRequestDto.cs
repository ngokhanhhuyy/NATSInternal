using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class BrandGetDetailRequestDto : IRequest<BrandGetDetailResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
}
