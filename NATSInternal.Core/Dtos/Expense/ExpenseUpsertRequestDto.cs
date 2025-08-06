namespace NATSInternal.Core.Dtos;

public class ExpenseUpsertRequestDto : IHasStatsUpsertRequestDto
{
    public long Amount { get; set; }
    public DateTime? StatsDateTime { get; set; }
    public ExpenseCategory Category { get; set; }
    public string Note { get; set; }
    public string PayeeName { get; set; }
    public List<ExpensePhotoRequestDto> Photos { get; set; }
    public string UpdatedReason { get; set; }
    
    public void TransformValues()
    {
        PayeeName = PayeeName?.ToNullIfEmpty();
        UpdatedReason = UpdatedReason?.ToNullIfEmpty();
        
        if (Photos != null)
        {
            foreach (ExpensePhotoRequestDto photo in Photos)
            {
                photo?.TransformValues();
            }
        }
    }
}