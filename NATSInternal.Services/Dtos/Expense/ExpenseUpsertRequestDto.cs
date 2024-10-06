namespace NATSInternal.Services.Dtos;

public class ExpenseUpsertRequestDto : IRequestDto
{
    public long Amount { get; set; }
    public DateTime? PaidDateTime { get; set; }
    public ExpenseCategory Category { get; set; }
    public string Note { get; set; }
    public string PayeeName { get; set; }
    public List<ExpensePhotoRequestDto> Photos { get; set; }
    public string UpdateReason { get; set; }
    
    public void TransformValues()
    {
        PayeeName = PayeeName?.ToNullIfEmpty();
        UpdateReason = UpdateReason?.ToNullIfEmpty();
        
        if (Photos != null)
        {
            foreach (ExpensePhotoRequestDto photo in Photos)
            {
                photo?.TransformValues();
            }
        }
    }
}