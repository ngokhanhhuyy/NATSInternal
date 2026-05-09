using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Payments;

public class PaymentListRequestDto : IHasStatsListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; }
    public string SortByFieldName { get; set; } = nameof(FieldToSort.StatsDate);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public ListMonthYearRequestDto? StatsMonthYear { get; set; }
    public int? CustomerId { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        if (CustomerId.HasValue && CustomerId.Value == 0)
        {
            CustomerId = null;
        }
    }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        StatsDate,
        Amount,
        CreatedDateTime,
        LastUpdatedDateTime
    }
    #endregion
}