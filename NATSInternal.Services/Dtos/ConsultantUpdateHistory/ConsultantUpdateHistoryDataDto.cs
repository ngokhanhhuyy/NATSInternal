namespace NATSInternal.Services.Dtos;

public class ConsultantUpdateHistoryDataDto
{
    public long AmountBeforeVat { get; set; }
    public long VatAmount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }

    internal ConsultantUpdateHistoryDataDto() { }
    
    internal ConsultantUpdateHistoryDataDto(Consultant consultant)
    {
        AmountBeforeVat = consultant.AmountBeforeVat;
        VatAmount = consultant.VatAmount;
        Note = consultant.Note;
        StatsDateTime = consultant.StatsDateTime;
    }
}