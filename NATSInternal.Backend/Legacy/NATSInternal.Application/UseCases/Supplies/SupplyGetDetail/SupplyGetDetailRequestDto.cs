using MediatR;

namespace NATSInternal.Application.UseCases.Supplies;

public class SupplyGetDetailRequestDto : IRequest<SupplyGetDetailResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
}