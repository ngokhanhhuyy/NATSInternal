namespace NATSInternal.Services.Dtos;

public class TreatmentListRequestDto : IOrderableListRequestDto, ILockableEntityListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; } = nameof(FieldOptions.PaidDateTime);
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IgnoreMonthYear { get; set; }
    public int? UserId { get; set; }
    public int? CustomerId { get; set; }
    public int? ProductId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    
    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty();
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();

        if (!IgnoreMonthYear)
        {
            if (Month == 0)
            {
                Month = currentDateTime.Month;
            }

            if (Year == 0)
            {
                Year = currentDateTime.Year;
            }
        }
    }
    
    public enum FieldOptions
    {
        PaidDateTime,
        Amount
    }
}