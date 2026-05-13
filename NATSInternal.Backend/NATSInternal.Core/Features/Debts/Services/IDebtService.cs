namespace NATSInternal.Core.Features.Debts;

public interface IDebtService
{
    #region Methods
    Task<DebtListRequestDto> GetListAsync(DebtListResponseDto responseDto);
    Task<DebtListResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(DebtCreateRequestDto requestDto);
    Task UpdateAsync(int id, DebtUpdateRequestDto requestDto);
    Task DeleteAsync(int id);
    #endregion
}
