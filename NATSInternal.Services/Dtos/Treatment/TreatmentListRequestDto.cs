namespace NATSInternal.Services.Dtos;

public class TreatmentListRequestDto : IProductExportableListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByField { get; set; }
    public ListMonthYearRequestDto MonthYear { get; set; }
    public int? CreatedUserId { get; set; }
    public int? CustomerId { get; set; }
    public int? ProductId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    
    public void TransformValues()
    {
        SortingByField = SortingByField?.ToNullIfEmpty();

        if (CreatedUserId == 0)
        {
            CreatedUserId = null;
        }

        if (CustomerId == 0)
        {
            CustomerId = null;
        }

        if (ProductId == 0)
        {
            ProductId = null;
        }
    }
}