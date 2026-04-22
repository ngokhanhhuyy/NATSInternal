using NATSInternal.Application.UseCases.Supplies;

namespace NATSInternal.Application.Services;

public interface ISupplyService
{
    #region Methods
    Task<SupplyGetListResponseDto> GetPaginatedSupplyListAsync(
        SupplyGetListRequestDto requestDto,
        CancellationToken cancellationToken = default);
    #endregion
}