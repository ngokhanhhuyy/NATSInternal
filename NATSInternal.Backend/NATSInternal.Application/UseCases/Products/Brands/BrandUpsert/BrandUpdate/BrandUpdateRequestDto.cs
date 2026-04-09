using MediatR;

namespace NATSInternal.Application.UseCases.Products;

public class BrandUpdateRequestDto : BrandUpsertRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
}