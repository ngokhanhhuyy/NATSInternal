namespace NATSInternal.Core.Features.Payments;

internal interface IPaymentInternalService : IPaymentService
{
    #region Methods
    Task<int> CreateWithoutValidationAsync(PaymentCreateRequestDto requestDto);
    Task UpdateWithoutValidationAsync(Payment payment, PaymentUpdateRequestDto requestDto);
    Task DeleteAsync(Payment payment);
    #endregion
}