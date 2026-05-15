namespace NATSInternal.Core.Features.Supplies;

public interface ISupplyService
{
    #region Methods
    Task<SupplyListResponseDto> GetListAsync(SupplyListRequestDto requestDto);
    Task<SupplyDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(AbstractSupplyUpsertRequestDto requestDto);
    Task UpdateAsync(int id, AbstractSupplyUpsertRequestDto requestDto);
    Task DeleteAsync(int id);
    #endregion
}
