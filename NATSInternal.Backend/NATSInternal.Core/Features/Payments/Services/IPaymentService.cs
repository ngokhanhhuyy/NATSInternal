namespace NATSInternal.Core.Features.Payments;

public interface IPaymentService
{
    #region Properties
    Task<PaymentListResponseDto> GetListAsync(PaymentListRequestDto requestDto);
    Task<PaymentDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(PaymentCreateRequestDto requestDto);
    Task UpdateAsync(int id, PaymentUpdateRequestDto requestDto);
    Task DeleteAsync(int id);
    #endregion
}