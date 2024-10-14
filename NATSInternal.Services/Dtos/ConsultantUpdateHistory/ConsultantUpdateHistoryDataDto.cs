namespace NATSInternal.Services.Dtos;

public class ConsultantUpdateHistoryDataDto
{
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }

    internal ConsultantUpdateHistoryDataDto() { }
    
    internal ConsultantUpdateHistoryDataDto(Consultant consultant)
    {
        Amount = consultant.AmountBeforeVat;
        Note = consultant.Note;
        StatsDateTime = consultant.StatsDateTime;
    }
}