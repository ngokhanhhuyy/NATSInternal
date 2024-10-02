namespace NATSInternal.Services.Dtos;

public class ConsultantUpdateHistoryDataDto
{
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime PaidDateTime { get; set; }

    internal ConsultantUpdateHistoryDataDto() { }
    
    internal ConsultantUpdateHistoryDataDto(Consultant consultant)
    {
        Amount = consultant.Amount;
        Note = consultant.Note;
        PaidDateTime = consultant.PaidDateTime;
    }
}