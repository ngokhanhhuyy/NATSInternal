namespace NATSInternal.Services.Dtos;

public class ConsultantUpsertRequestDto : IFinancialEngageableUpsertRequestDto
{
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime? StatsDateTime { get; set; }
    public int CustomerId { get; set; }
    public CustomerUpsertRequestDto Customer { get; set; }
    public string UpdatedReason { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
    }
}