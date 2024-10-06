namespace NATSInternal.Services.Dtos;

public class CustomerListRequestDto : IRequestDto
{
    public bool OrderByAscending { get; set; } = true;
    public string OrderByField { get; set; } = nameof(FieldToBeOrdered.LastName);
    public string SearchByContent { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public bool HasRemainingDebtAmountOnly { get; set; }

    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty();
        SearchByContent = SearchByContent?.ToNullIfEmpty();
    }

    public enum FieldToBeOrdered
    {
        LastName,
        FullName,
        Birthday,
        CreatedDateTime,
        DebtRemainingAmount
    }
}