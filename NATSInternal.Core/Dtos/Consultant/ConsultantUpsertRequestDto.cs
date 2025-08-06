namespace NATSInternal.Core.Dtos;

public class ConsultantUpsertRequestDto : IHasStatsUpsertRequestDto
{
    public long AmountBeforeVat { get; set; }
    public long VatAmount { get; set; }
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