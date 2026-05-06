namespace NATSInternal.Core.Features.Expenses;

public interface IExpenseService
{
    #region Methods
    Task<ExpenseListResponseDto> GetListAsync(ExpenseListRequestDto requestDto);
    Task<ExpenseDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(ExpenseUpsertRequestDto requestDto);
    Task UpdateAsync(int id, ExpenseUpsertRequestDto requestDto);
    Task DeleteAsync(int id);
    #endregion
}