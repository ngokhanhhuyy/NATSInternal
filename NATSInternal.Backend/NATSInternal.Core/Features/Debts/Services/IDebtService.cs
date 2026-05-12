namespace NATSInternal.Core.Features.Debts;

public interface IDebtService
{
    #region Methods
    Task<DebtListRequestDto> GetListAsync(DebtListResponseDto responseDto);
    Task<DebtListResponseDto> GetDetailAsync(int id);
    #endregion
}
