namespace NATSInternal.Core.Features.Debts;

public interface IDebtInternalService : IDebtService
{
    #region Methods
    Task CreateAsync(DebtCreateRequestDto requestDto);
    Task UpdateOrDeleteAsync(int id, DebtUpdateRequestDto requestDto);
    #endregion
}
