using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Orders;

public class OrderListRequestDto : IHasStatsListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = false;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.StatsDate);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public OrderType? Type { get; set; }
    public int? CustomerId { get; set; }
    public int? ProductId { get; set; }
    public OrderPaymentStatus? PaymentStatus { get; set; }
    public int? StatsYear { get; set; }
    public int? StatsMonth { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        StatsDate,
        CreatedDateTime,
        LastUpdatedDateTime,
        Amount
    }

    public enum OrderPaymentStatus
    {
        Debt,
        RefundNeeded
    }
    #endregion
}
