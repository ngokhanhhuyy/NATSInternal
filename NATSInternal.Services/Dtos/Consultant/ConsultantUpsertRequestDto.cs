namespace NATSInternal.Services.Dtos;

public class ConsultantUpsertRequestDto : IRequestDto
{
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime? PaidDateTime { get; set; }
    public int CustomerId { get; set; }
    public CustomerUpsertRequestDto Customer { get; set; }
    public string UpdateReason { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
    }
}